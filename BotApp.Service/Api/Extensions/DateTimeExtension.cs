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
