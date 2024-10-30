using BotApp.Service.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Data
{
    internal class SunriseSunset
    {
        public DateTime? Sunrise { get; private set; } = null;
        public DateTime? Sunset { get; private set; } = null;
        public DateTime? SolarNoon { get; private set; } = null;
        public int? DayLength { get; private set; } = null;
        public DateTime? CivilTwilightBegin { get; private set; } = null;            
        public DateTime? CivilTwilightEnd { get; private set; } = null;
        public DateTime? NauticalTwilightBegin { get; private set; } = null;
        public DateTime? NauticalTwilightEnd { get; private set; } = null;
        public DateTime? AstronomicalTwilightBegin { get; private set; } = null;
        public DateTime? AstronomicalTwilightEnd { get; private set; } = null;  

        private SunriseSunset() { }

        private static DateTime? ToDateTime(string? value, string? tzid)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;  

            //var dateFormatString = "yyyy-MM-ddTHH:mm:ss";
            //var dateFormatString = "yyyy-MM-ddTHH:mm:sszzz";
            var dateTimeKind =
                (tzid ?? "UTC").Equals("UTC", StringComparison.OrdinalIgnoreCase)
                    ? DateTimeKind.Utc
                    : DateTimeKind.Local;

            DateTimeOffset dateTimeOffice;

            if (DateTimeOffset.TryParse(value, out dateTimeOffice))
            {
                if (dateTimeKind == DateTimeKind.Utc)
                    return dateTimeOffice.UtcDateTime;
                
                if (dateTimeKind == DateTimeKind.Local)
                    return dateTimeOffice.LocalDateTime;

                return dateTimeOffice.DateTime;
            }

            return null;
        }

        public static SunriseSunset Create(ResultEntity? resultEntity)
        {
            resultEntity.ThrowIfNull();

            return new SunriseSunset()
            {
                Sunrise = ToDateTime(resultEntity?.results?.sunrise, resultEntity?.tzid),
                Sunset = ToDateTime(resultEntity?.results?.sunset, resultEntity?.tzid),
                SolarNoon = ToDateTime(resultEntity?.results?.solar_noon, resultEntity?.tzid),
                DayLength = Convert.ToInt32(resultEntity?.results?.day_length),
                CivilTwilightBegin = ToDateTime(resultEntity?.results?.civil_twilight_begin, resultEntity?.tzid),
                CivilTwilightEnd = ToDateTime(resultEntity?.results?.civil_twilight_end, resultEntity?.tzid),
                NauticalTwilightBegin = ToDateTime(resultEntity?.results?.nautical_twilight_begin, resultEntity?.tzid),
                NauticalTwilightEnd = ToDateTime(resultEntity?.results?.nautical_twilight_end, resultEntity?.tzid),
                AstronomicalTwilightBegin = ToDateTime(resultEntity?.results?.astronomical_twilight_begin, resultEntity?.tzid),
                AstronomicalTwilightEnd = ToDateTime(resultEntity?.results?.astronomical_twilight_end, resultEntity?.tzid),
            };
        }
    }
}
