using BotApp.Logger.Api;
using BotApp.Service.Api.Consts;
using BotApp.Service.Api.Data;
using BotApp.Service.Api.Enums;
using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using BotApp.Service.Api.Helpers;
using BotApp.Service.Api.SunrizeSunet.HttpLogger;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using BotApp.Telegram.Api.Utils;
using Microsoft.VisualBasic;

namespace BotApp.Service.Api.SunrizeSunet
{
    internal static class SunriseSunsetHelper
    {
        public readonly static Dictionary<DayOfWeek, Planet> PlanetByDayOfWeek = new()
        {
            { DayOfWeek.Sunday, Planet.SUN },
            { DayOfWeek.Monday, Planet.MOON },
            { DayOfWeek.Tuesday, Planet.MARS },
            { DayOfWeek.Wednesday, Planet.MERCURY },
            { DayOfWeek.Thursday, Planet.JUPITER },
            { DayOfWeek.Friday, Planet.VENUS },
            { DayOfWeek.Saturday, Planet.SATURN },
        };

        public readonly static Dictionary<DayOfWeek, Planet> PlanetByNightOfWeek = new()
        {
            { DayOfWeek.Sunday, Planet.JUPITER },
            { DayOfWeek.Monday, Planet.VENUS },
            { DayOfWeek.Tuesday, Planet.SATURN },
            { DayOfWeek.Wednesday, Planet.SUN },
            { DayOfWeek.Thursday, Planet.MOON },
            { DayOfWeek.Friday, Planet.MARS },
            { DayOfWeek.Saturday, Planet.MERCURY },

        };

        public readonly static List<Planet> PlanetsList = new()
        {
            Planet.SUN ,
            Planet.VENUS ,
            Planet.MERCURY ,
            Planet.MOON ,
            Planet.SATURN ,
            Planet.JUPITER ,
            Planet.MARS ,
        };

        internal static Uri MakeUri(Location location, DateTime dateTime)
        {
#if DEBUG
            LoggerService.Logger?.LogDebug($"{nameof(location)} = {location.ToString()}");
            LoggerService.Logger?.LogDebug($"{nameof(dateTime)} = {dateTime.ToString()}");
#endif
            var buildUri = new UriBuilder(SunriseSunsetConsts.BASE_URL);
            
            buildUri.Path = SunriseSunsetConsts.PATH_JSON;
            
            var culture = CultureInfo.GetCultureInfo("en_US");

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(SunriseSunsetConsts.PARAM_LAT, location.Latitude.ToString(culture.NumberFormat));
            queryString.Add(SunriseSunsetConsts.PARAM_LNG, location.Longitude.ToString(culture.NumberFormat));
            queryString.Add(SunriseSunsetConsts.PARAM_DATE, dateTime.ToString("yyyy-MM-dd"));
            queryString.Add(SunriseSunsetConsts.PARAM_FORMATTED, "0");

            buildUri.Query = queryString.ToString();

            var uri = buildUri.Uri;
            
#if DEBUG
            LoggerService.Logger?.LogDebug($"Uri = {uri.ToString()}");
#endif
            LoggerService.Logger?.LogInformation($"Created uri {uri.ToString()}");

            return uri;
        }

        internal static Uri MakeUriToday(Location location)
        {
            var today = DateTime.Now;

            return MakeUri(location, today);
        }

        internal static Uri MakeUriYesterday(Location location)
        {
            var today = DateTime.Now;
            var timeSpan = new TimeSpan(1, 0, 0, 0, 0);
            return MakeUri(location, today - timeSpan);
        }

        internal static Uri MakeUriTomorrow(Location location)
        {
            var today = DateTime.Now;
            var timeSpan = new TimeSpan(1, 0, 0, 0, 0);
            return MakeUri(location, today + timeSpan);
        }

        internal static async Task<SunriseSunset> GetJSON(Uri address)
        {
            using (var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler())))
            {
#if DEBUG
                LoggerService.Logger?.LogDebug($"{nameof(address)} = {address.ToString()}");
#endif
                var cts = new CancellationTokenSource();

                LoggerService.Logger?.LogInformation($"Receiving JSON data from {address.ToString()}");
                var result = await httpClient.GetFromJsonAsync<ResultEntity>(address, cts.Token);
                var status = result?.status ?? string.Empty;

                if (status != SunriseSunsetConsts.STATUS_OK)
                {
                    LoggerService.Logger?.LogError($"Incorrect answer from {address.ToString()} ({status})");
                    throw new Exception($"Incorent answer from {address.ToString()}");
                }

                return SunriseSunset.Create(resultEntity: result);
            }
        }

        internal static string GetPlanetImage(Planet planet)
        {
            switch (planet)
            {
                case Planet.SUN: return "☉";
                case Planet.MOON: return "☽";
                case Planet.VENUS: return "♀︎";
                case Planet.MERCURY: return "☿";
                case Planet.MARS: return "♂︎";
                case Planet.JUPITER: return "♃";
                case Planet.SATURN: return "♄";
                case Planet.PLUTO: return "♂︎";
            }

            return string.Empty;
        }

        internal static Planet GetPlanet(IDictionary<DayOfWeek, Planet> planets, DateTime date)
        {
            if (planets.ContainsKey(date.DayOfWeek))
                return planets[date.DayOfWeek];

            return Planet.NONE;
        }
        internal static Planet GetPlanet(DateTime date)
        {
            return GetPlanet(PlanetByDayOfWeek, date);
        }

        internal static int GetPlanetIndex(DateTime date, int hourIndex, bool isTwilling)
        {
            var planetsDict = (isTwilling ? PlanetByNightOfWeek : PlanetByDayOfWeek);
            var planet = GetPlanet(planetsDict, date);
#if DEBUG
            LoggerService.Logger?.LogDebug($"planet = {planet.ToString()}");
#endif

            var startIndex = PlanetsList.IndexOf(planet);

            if (startIndex == -1)
                throw new KeyNotFoundException($"Key {planet.ToString()} not found.");

#if DEBUG
            LoggerService.Logger?.LogDebug($"startIndex = {startIndex} (0..{PlanetsList.Count - 1})");
#endif

            var index = ((startIndex + hourIndex) % PlanetsList.Count);

#if DEBUG
            LoggerService.Logger?.LogDebug($"index = {index} (0..{PlanetsList.Count - 1})");
#endif

            if (index < 0)
                index += PlanetsList.Count - 1;

#if DEBUG
            LoggerService.Logger?.LogDebug($"index = {index} (0..{PlanetsList.Count - 1})");
#endif

            if (index >= PlanetsList.Count)
                index -= PlanetsList.Count;

#if DEBUG
            LoggerService.Logger?.LogDebug($"index = {index} (0..{PlanetsList.Count - 1})");
#endif

            return index;
        }

        internal static Planet GetPlanet(DateTime date, int hourIndex, bool isTwilling)
        {
            var index = GetPlanetIndex(date, hourIndex, isTwilling);

            return PlanetsList[index];
        }

        private static string GetDateTimeString(DateTime dateTime, DateTime today)
        {
            var dateFormat = string.Empty;

            if (GlobalServiceData.TimeZone != null)
            {
                dateTime = TimeZoneInfo.ConvertTime(dateTime, GlobalServiceData.TimeZone);
                today = today.ApplyTimeZone(GlobalServiceData.TimeZone);
            }

            dateFormat = dateTime.Date == today.Date ? DateTimeConsts.TIME_FORMAT : DateTimeConsts.LONG_DATE_FORMAT;

            return dateTime.ToSlavicString(dateFormat);
        }
        private static string GetDateTimeString(DateTime date, string dateFormat)
        {
            var dateTime = date;

            if (GlobalServiceData.TimeZone != null)
                dateTime = TimeZoneInfo.ConvertTime(date, GlobalServiceData.TimeZone);

            return dateTime.ToSlavicString(dateFormat);
        }

        private static async Task ProcessShowSunriseSunsetInfo(
            ITelegramBotClient botClient,
            Update update,
            DateTime sunrise,
            DateTime sunset,
            DateTime datetime,
            bool isTwilling,
            bool showFullPlanetsTable,
            CancellationToken cancellationToken)
        {
#if DEBUG
            LoggerService.Logger?.LogDebug($"{nameof(datetime)} = {datetime}");
            LoggerService.Logger?.LogDebug($"{nameof(sunrise)}  = {sunrise}");
            LoggerService.Logger?.LogDebug($"{nameof(sunset)}  = {sunset}");
#endif
            var stringBuilder = new StringBuilder();

            try
            {
                var id = Global.GetId(update);
                var location = GlobalServiceData.GetLocation(id);

                var delta = sunset - sunrise;
                var ticks = delta.Ticks;
                var hourLengthTicks = ticks / 12;
                var hourLength = new TimeSpan(hourLengthTicks);
                var planets = isTwilling ? SunriseSunsetHelper.PlanetByNightOfWeek : SunriseSunsetHelper.PlanetByDayOfWeek;

                var planetOfDay = SunriseSunsetHelper.GetPlanet(planets, datetime);
                var planetOfDayImage = SunriseSunsetHelper.GetPlanetImage(planetOfDay);

                stringBuilder.Clear();

                var header = string.Format("{0} {1}", isTwilling ? "☾" : "☼", GetDateTimeString(datetime, DateTimeConsts.LONG_DATE_FORMAT));
                stringBuilder.AppendLine(string.Format(Properties.Resources.BoldFmt, header));
                stringBuilder.AppendLine(Functions.LocationToString(location));
                stringBuilder.AppendLine();

                if (isTwilling)
                {
                    stringBuilder.AppendLine(string.Format(Properties.Resources.SunsetFmt, GetDateTimeString(sunrise, datetime)));
                    stringBuilder.AppendLine(string.Format(Properties.Resources.SunriseFmt, GetDateTimeString(sunset, datetime)));

                    stringBuilder.AppendLine(string.Format(Properties.Resources.NightLengthFmt, new TimeSpan(ticks).ToString(DateTimeConsts.TIMESPAN_FORMAT)));
                }
                else
                {
                    stringBuilder.AppendLine(string.Format(Properties.Resources.SunriseFmt, GetDateTimeString(sunrise, datetime)));
                    stringBuilder.AppendLine(string.Format(Properties.Resources.SunsetFmt, GetDateTimeString(sunset, datetime)));

                    stringBuilder.AppendLine(string.Format(Properties.Resources.DayLengthFmt, new TimeSpan(ticks).ToString(DateTimeConsts.TIMESPAN_FORMAT)));
                }

                stringBuilder.AppendLine(string.Format(Properties.Resources.HourLengthFmt, hourLength.ToString(DateTimeConsts.TIMESPAN_FORMAT)));
                stringBuilder.AppendLine(string.Format(Properties.Resources.DayOfWeekFmt, datetime.DayOfWeek, planetOfDayImage));

                stringBuilder.AppendLine();

                if (showFullPlanetsTable)
                {
                    for (int index = 0; index < 12; index++)
                    {
                        var startDateTime = sunrise + new TimeSpan(index * hourLengthTicks);
                        var endDateTime = sunrise + new TimeSpan((index + 1) * hourLengthTicks);

                        if (index == 0) startDateTime = sunrise;
                        if (index == 11) endDateTime = sunset;

                        var planetOfHour = SunriseSunsetHelper.GetPlanet(datetime, index, isTwilling);
                        var planetOfHourImage = SunriseSunsetHelper.GetPlanetImage(planetOfHour);

                        stringBuilder.AppendLine(string.Format(Properties.Resources.TableRowFmt,
                            (index + 1),
                            planetOfHourImage,
                            GetDateTimeString(startDateTime, datetime),
                            GetDateTimeString(endDateTime, datetime)));
                    }

                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("<code>");

                    var elements = Enum.GetValues<Planet>();

                    foreach (var element in elements)
                    {
                        if (element != Planet.NONE)
                        {
                            stringBuilder.AppendLine(string.Format("{0} - {1}",
                                SunriseSunsetHelper.GetPlanetImage(element),
                                element.ToString().ToCamelCase()));
                        }
                    }

                    stringBuilder.AppendLine("</code>");
                }
                else
                {
                    if (datetime >= sunrise && datetime <= sunset)
                    {
                        var index = Math.Max(0, Math.Min(11, ((datetime.Ticks - sunrise.Ticks) / hourLengthTicks)));

                        var startDateTime = sunrise + new TimeSpan(index * hourLengthTicks);
                        var endDateTime = sunrise + new TimeSpan((index + 1) * hourLengthTicks);

                        if (index == 0) startDateTime = sunrise;
                        if (index == 11) endDateTime = sunset;

                        var planetOfHour = SunriseSunsetHelper.GetPlanet(datetime, (int)index, isTwilling);
                        var planetImage = SunriseSunsetHelper.GetPlanetImage(planetOfHour);

                        stringBuilder.AppendLine(string.Format(Properties.Resources.TableRowFmt,
                            (index + 1),
                            planetImage,
                            GetDateTimeString(startDateTime, datetime),
                            GetDateTimeString(endDateTime, datetime)));

                        stringBuilder.AppendLine();
                        stringBuilder.AppendLine("<code>");

                        stringBuilder.AppendLine(string.Format("{0} - {1}",
                            SunriseSunsetHelper.GetPlanetImage(planetOfDay),
                            planetOfDay.ToString().ToCamelCase()));

                        stringBuilder.AppendLine(string.Format("{0} - {1}",
                            SunriseSunsetHelper.GetPlanetImage(planetOfHour),
                            planetOfHour.ToString().ToCamelCase()));

                        stringBuilder.AppendLine("</code>");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerService.Logger?.LogError(ex, "Exception: {Message}", ex.Message);
                stringBuilder.Clear();
                stringBuilder.Append(string.Format(Properties.Resources.ExceptionMessageFmt, ex.Message));
            }

#if DEBUG
            LoggerService.Logger?.LogDebug(stringBuilder.ToString());
#endif
            await botClient.SendTextMessageAsync(Global.GetId(update), stringBuilder.ToString(), parseMode: ParseMode.Html);
        }

        internal static async Task SendSunriseSunsetInfo(
            ITelegramBotClient botClient, 
            Update update, 
            DateTime datetime,
            bool showFullPlanetsTable,
            CancellationToken cancellationToken)
        {
            var id = Global.GetId(update);           
            var location = GlobalServiceData.GetLocation(id);

            var address = SunriseSunsetHelper.MakeUri(location ?? throw new ArgumentNullException(), datetime);
            var sunriseSunset = await SunriseSunsetHelper.GetJSON(address);

            if (sunriseSunset == null) return;
            if (sunriseSunset.Sunrise == null) return;
            if (sunriseSunset.Sunset == null) return;

            var sunrise = sunriseSunset.Sunrise ?? throw new ArgumentNullException(nameof(sunriseSunset.Sunrise));
            var sunset = sunriseSunset.Sunset ?? throw new ArgumentNullException(nameof(sunriseSunset.Sunset));

            var isTwilling = false;

            if (!showFullPlanetsTable)
            {
                if (datetime < sunrise)
                {
                    var yesterday = datetime.AddDays(-1);

                    Uri uri = MakeUri(Helper.ThrowIfNull(location), yesterday);
                    var res = await GetJSON(uri);

                    sunset = sunrise;
                    sunrise = res.Sunrise ?? throw new ArgumentNullException(nameof(sunriseSunset.Sunrise));

                    isTwilling = true;
                }
                else if (datetime > sunset)
                {
                    var tomorrow = datetime.AddDays(1);

                    var uri = MakeUri(Helper.ThrowIfNull(location), tomorrow);
                    var res = await GetJSON(uri);

                    sunrise = sunset;
                    sunset = res.Sunrise ?? throw new ArgumentNullException(nameof(sunriseSunset.Sunrise));

                    isTwilling = true;
                }
            }

            await ProcessShowSunriseSunsetInfo(
                botClient,
                update,
                sunrise,
                sunset!,
                datetime,
                isTwilling,
                showFullPlanetsTable,
                cancellationToken);
        }
    }
}
