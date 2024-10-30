using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime ApplyTimeZone(this DateTime dateTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }
    }
}
