using System.Globalization;

namespace Rollback.Common.Extensions
{
    public static class TimeExtensions
    {
        public static int GetUnixTimeStamp(this DateTime date) =>
            (int)date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()).TotalSeconds;

        public static DateTime GetDateTimeFromUnixTimeStamp(this int timeStamp) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(timeStamp);

        public static bool IsSameWeek(this DateTime date, DateTime other) =>
            CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay,
                CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) ==
                CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(other, CalendarWeekRule.FirstDay,
                CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
    }
}
