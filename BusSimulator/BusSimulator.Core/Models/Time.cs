using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BusSimulator.Core.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Time
    {
        /// <summary>
        /// Creates a new instance of <see cref="Time"/>
        /// </summary>
        /// <param name="totalSeconds">The amount of seconds passed from midnight</param>
        [JsonConstructor]
        public Time(int totalSeconds)
        {
            this.TotalSeconds = totalSeconds;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Time"/>
        /// </summary>
        /// <param name="dateTime">The datetime whose time component will be extracted</param>
        public Time(DateTime dateTime)
        {
            this.TotalSeconds = (int) (dateTime - dateTime.Date).TotalSeconds;
        }

        /// <summary>
        /// The hour component
        /// </summary>
        public int Hour { get => this.TotalSeconds / 3600 % 60 % 24; }

        /// <summary>
        /// The minute component
        /// </summary>
        public int Minute { get => this.TotalSeconds / 60 % 60; }

        /// <summary>
        /// The second component
        /// </summary>
        public int Second { get => this.TotalSeconds % 60; }

        /// <summary>
        /// The overlap
        /// </summary>
        public int Overlap { get => this.TotalSeconds / 86400; }

        /// <summary>
        /// The total minutes
        /// </summary>
        [JsonProperty]
        public int TotalSeconds { get; set; }

        /// <summary>
        /// Converts the time to a datetime
        /// </summary>
        /// <returns>The date time</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(5000, 1, 1, this.Hour, this.Minute, this.Second);
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return this.Hour.ToString("00") + ":" + this.Minute.ToString("00") + ":" + this.Second.ToString("00");
        }

        /// <summary>
        /// Returns a string that represents the current time only by using hours and minutes
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public string ToShortString()
        {
            return this.Hour.ToString("00") + ":" + this.Minute.ToString("00");
        }

        /// <summary>
        /// The short string to be readable by WPF
        /// </summary>
        public string ShortString { get => this.ToShortString(); }

        /// <summary>
        /// Adds the given timespan to the time
        /// </summary>
        /// <param name="a">The time</param>
        /// <param name="b">The timespan</param>
        /// <returns>The new time</returns>
        public static Time operator +(Time a, TimeSpan b)
        {
            return new Time(a.TotalSeconds + (int) b.TotalSeconds);
        }

        /// <summary>
        /// Adds the given minutes to the time
        /// </summary>
        /// <param name="a">The time</param>
        /// <param name="b">The minutes</param>
        /// <returns>The new time</returns>
        public static Time operator +(Time a, int b)
        {
            return new Time(a.TotalSeconds + (b * 60));
        }

        /// <summary>
        /// Subtracts the given time from another time
        /// </summary>
        /// <param name="a">The first time component</param>
        /// <param name="b">The second time component</param>
        /// <returns>The difference</returns>
        public static TimeSpan operator -(Time a, Time b)
        {
            return TimeSpan.FromSeconds(a.TotalSeconds - b.TotalSeconds);
        }

        /// <summary>
        /// Compares if <paramref name="a"/> is smaller than <paramref name="b"/>
        /// </summary>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>The comparison result</returns>
        public static bool operator <(Time a, Time b)
        {
            return a.TotalSeconds < b.TotalSeconds;
        }

        /// <summary>
        /// Compares if <paramref name="a"/> is greater than <paramref name="b"/>
        /// </summary>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>The comparison result</returns>
        public static bool operator >(Time a, Time b)
        {
            return a.TotalSeconds > b.TotalSeconds;
        }

        /// <summary>
        /// Compares if <paramref name="a"/> is smaller or equal than <paramref name="b"/>
        /// </summary>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>The comparison result</returns>
        public static bool operator <=(Time a, Time b)
        {
            return a.TotalSeconds <= b.TotalSeconds;
        }

        /// <summary>
        /// Compares if <paramref name="a"/> is greater or equal than <paramref name="b"/>
        /// </summary>
        /// <param name="a">The first value</param>
        /// <param name="b">The second value</param>
        /// <returns>The comparison result</returns>
        public static bool operator >=(Time a, Time b)
        {
            return a.TotalSeconds >= b.TotalSeconds;
        }

        /// <summary>
        /// Generates a frequence of times in the given time range with the given interval
        /// </summary>
        /// <param name="start">The start time</param>
        /// <param name="interval">The interval in minutes</param>
        /// <returns>The enumeration of times</returns>
        public static IEnumerable<Time> Frequence(Time start, Time end, int interval)
        {
            return Frequence(start, interval, ((int) (end - start).TotalMinutes / interval) + 1);
        }

        /// <summary>
        /// Generates a frequence of times since the given starting time for the given count
        /// </summary>
        /// <param name="start">The start time</param>
        /// <param name="interval">The interval in minutes</param>
        /// <param name="count">The amount of times to repeat the frequence</param>
        /// <returns>The enumeration of times</returns>
        public static IEnumerable<Time> Frequence(Time start, int interval, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Time(start.TotalSeconds + (interval * i * 60));
            }
        }

        /// <summary>
        /// The <see langword="null"/> value
        /// </summary>
        public static Time Null = new Time(-1);
    }
}
