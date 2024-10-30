using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BotApp.Service.Api.Utils
{
    internal static class GlobalServiceData
    {
        private readonly static IDictionary<long, Location?> locations = new Dictionary<long, Location?>();

        internal static TimeZoneInfo? TimeZone { get; set; } = null;
        internal static Location? Location { get; set; } = null;
        internal static double Latitude  => Location?.Latitude ?? 0;
        internal static double Longitude => Location?.Longitude ?? 0;

        internal static string? TimeZoneId { get; set; } = null;

        internal static Location? GetLocation(long id)
        {
            if (id == 0) return null;

            if (locations.ContainsKey(id)) return locations[id];

            return null;
        }
        internal static void SetLocation(long id, Location? location)
        {
            locations[id] = location;
        }
        internal static double GetLatitude(long id)
        {
            var location = GetLocation(id);

            return location?.Latitude ?? 0;
        }
        internal static double GetLongitude(long id)
        {
            var location = GetLocation(id);

            return location?.Longitude ?? 0;
        }
    }
}
