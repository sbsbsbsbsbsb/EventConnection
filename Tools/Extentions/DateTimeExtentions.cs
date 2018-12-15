using System;

namespace Tools.Extentions
{
    public static class DateTimeExtentions
    {
        public static DateTime GetDateFromTimeSpan(int timaSpan)
        {
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromSeconds(timaSpan));
        }

        public static int GetUnixTimeStamp(this DateTime date)
        {
            return (int) (date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
