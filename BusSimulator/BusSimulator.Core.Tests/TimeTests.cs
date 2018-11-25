using BusSimulator.Core.Models;

using System;

using Xunit;

namespace BusSimulator.Core.Tests
{
    public class TimeTests
    {
        [Fact]
        public void Time_Properties_Correct()
        {
            var time = new Time(46800 + 1800 + 15);

            Assert.Equal(13, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(15, time.Second);
        }

        [Fact]
        public void Time_Overlap_Correct()
        {
            var time = new Time(86400 + 46800 + 1800 + 15);

            Assert.Equal(1, time.Overlap);
            Assert.Equal(13, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(15, time.Second);
        }

        [Fact]
        public void Time_Second_Correct()
        {
            var time = new Time(30);

            Assert.Equal(30, time.Second);
        }

        [Fact]
        public void Time_Minute_Correct()
        {
            var time = new Time(60);

            Assert.Equal(1, time.Minute);
        }

        [Fact]
        public void Time_Hour_Correct()
        {
            var time = new Time(3600);

            Assert.Equal(1, time.Hour);
        }

        [Fact]
        public void Time_ToString_OnlyZeros()
        {
            var time = new Time(0);

            Assert.Equal("00:00:00", time.ToString());
        }

        [Fact]
        public void Time_ToString_LeadingZeros()
        {
            var time = new Time(90);

            Assert.Equal("00:01:30", time.ToString());
        }

        [Fact]
        public void Time_ToString_Normal()
        {
            var time = new Time(671);

            Assert.Equal("00:11:11", time.ToString());
        }

        [Fact]
        public void Time_Constructor_DateTime()
        {
            var dateTime = new DateTime(2018, 11, 14, 20, 30, 40);
            var time = new Time(dateTime);

            Assert.Equal(20, time.Hour);
            Assert.Equal(30, time.Minute);
            Assert.Equal(40, time.Second);
            Assert.Equal(73840, time.TotalSeconds);
        }
    }
}
