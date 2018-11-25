using System;

namespace BusSimulator.Core.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToTimeString(this TimeSpan time)
        {
            return (time.Ticks < 0 ? "-" : string.Empty) + Math.Abs(time.TotalSeconds < 60 ? 0 : (int) time.TotalMinutes).ToString("00") + ":" + Math.Abs(time.Seconds).ToString("00");
        }
    }
}
