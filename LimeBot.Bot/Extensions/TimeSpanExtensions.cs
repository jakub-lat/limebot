using System;

namespace LimeBot.Bot.Extensions
{
    public static class TimespanExtensions
    {
        public static string ToHumanReadableString(this TimeSpan t)
        {
            if (t.TotalSeconds <= 1)
            {
                return $@"{t:s\.ff} seconds";
            }
            if (t.TotalMinutes < 1)
            {
                return $@"{t:%s} seconds";
            }
            if(t.TotalMinutes >= 1 && t.TotalMinutes < 1.5)
            {
                return $@"1 minute";
            }
            if (t.TotalHours <= 1)
            {
                return $@"{t:%m} minutes";
            }
            if (t.TotalDays <= 1)
            {
                return $@"{t:%h} hours";
            }

            return $@"{t:%d} days";
        }
    }
}
