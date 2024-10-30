using BotApp.Service.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace BotApp.Service.Api.Utils
{
    internal static class Functions
    {
        internal static string CoordinateToSting(double coordinate)
        {
            var degrees = (int)coordinate;
            var minutes = (int)((coordinate - degrees) * 60.0);
            var seconds = (int)((coordinate - degrees - minutes / 60.0) * 3600.0);

            return string.Format("{0:00}°{1:00}'{2:00}", degrees, minutes, seconds);
        }
        internal static string CoordinatesToSting(double latitude, double longitude)
        {
            var latDir = (latitude < 0) ? "S" : "N";
            var lonDir = (longitude < 0) ? "W" : "E";

            var latStr = CoordinateToSting(latitude);
            var lonStr = CoordinateToSting(longitude);  

            return string.Format("{0}{1} {2}{3}", latStr, latDir, lonStr, lonDir);
        }
        internal static string LocationToString(Location? location) => CoordinatesToSting(location?.Latitude ?? 0.0, location?.Longitude ?? 0.0);

        internal static string ChoiceDateFormate(DateTime dateTime, string todauFmt, string otherFmt)
        {
            return dateTime.ThrowIfNull(nameof(dateTime)).Date == DateTime.UtcNow.Date ? todauFmt : otherFmt;
        }
    }
}
