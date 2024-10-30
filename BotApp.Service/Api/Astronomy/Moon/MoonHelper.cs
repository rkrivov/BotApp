using BotApp.Service.Api.Consts;
using Telegram.Bot;
using Telegram.Bot.Types;

using TimeAndDate.Services;
using TimeAndDate.Services.DataTypes.Astro;
using TimeAndDate.Services.DataTypes.Places;


namespace BotApp.Service.Api.Astronomy.Moon
{
    internal static class MoonHelper
    {
        public static async Task GetMoonPhase(decimal latitude, decimal longtitude, DateTime dateTime)
        {
            var service = new AstronomyService(AuthorizeTokensConsts.TDAccessKey, AuthorizeTokensConsts.TDSecretKey);

            var coordinates = new Coordinates(latitude, longtitude);

            var placeId = new LocationId(coordinates);
            var result = await service.GetAstronomicalInfoAsync(AstronomyObjectType.Moon, placeId, dateTime);
        }
        public static async Task GetMoonPhase(decimal latitude, decimal longtitude, DateTime startDateTime, DateTime endDateTime)
        {
            var service = new AstronomyService(AuthorizeTokensConsts.TDAccessKey, AuthorizeTokensConsts.TDSecretKey);

            var coordinates = new Coordinates(latitude, longtitude);

            var placeId = new LocationId(coordinates);
            var result = await service.GetAstronomicalInfoAsync(AstronomyObjectType.Moon, placeId, startDateTime, endDateTime);
        }
    }
}
