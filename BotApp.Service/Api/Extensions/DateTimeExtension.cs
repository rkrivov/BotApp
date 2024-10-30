namespace BotApp.Service.Api.Extensions
{
    internal static class DateTimeOffsetExtension
    {
        internal static string ToSlavicString(this DateTimeOffset dateTimeOffset)
        {
            var slavicDateTimeOffset = dateTimeOffset.AddYears(5509);

            return slavicDateTimeOffset.ToString("dd.MM.yyyy HH:mm:ss");
        }
        internal static string ToSlavicString(this DateTimeOffset dateTimeOffset, string format)
        {
            var slavicDateTimeOffset = dateTimeOffset.AddYears(5509);

            return slavicDateTimeOffset.ToString(format);
        }
        internal static DateTime ApplyTimeZone(this DateTime dateTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        internal static DateTime GetDayStart(this DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = dateTime.Day;

            return new DateTime(year, month, day, 0, 0, 0);
        }
        internal static DateTime GetDayEnd(this DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = dateTime.Day;

            return new DateTime(year, month, day, 23, 59, 59);
        }
        internal static DateTime GetMonthStart(this DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;

            return new DateTime(year, month, 1, 0, 0, 0);
        }
        internal static DateTime GetMonthEnd(this DateTime dateTime)
        {
            var endOfMonth = dateTime.AddMonths(1);
            endOfMonth = endOfMonth.GetMonthStart();
            endOfMonth = endOfMonth.AddDays(-1);

            return endOfMonth.GetDayEnd();
        }
        internal static DateTime GetYearStart(this DateTime dateTime) => new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
        internal static DateTime GetYearEnd(this DateTime dateTime) => new DateTime(dateTime.Year, 12, 31, 23, 59, 59);
    }
    internal static class DateTimeExtension
    {
        internal static string ToSlavicString(this DateTime dateTime)
        {
            var slavicDate = dateTime.AddYears(5509);

            return slavicDate.ToString("dd.MM.yyyy HH:mm:ss");
        }

        internal static string ToSlavicString(this DateTime dateTime, string format)
        {
            var slavicDate = dateTime.AddYears(5509);

            return slavicDate.ToString(format);
        }
        internal static string ToSlavicString(this DateTime dateTime, TimeSpan offset)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime, offset);

            return dateTimeOffset.ToSlavicString("dd.MM.yyyy HH:mm:ss");
        }

        internal static string ToSlavicString(this DateTime dateTime, TimeSpan offset, string format)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime, offset);

            return dateTimeOffset.ToSlavicString(format);
        }
    }

}
