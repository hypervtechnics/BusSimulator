using System;
using System.Collections.Generic;

namespace BusSimulator.Core.Models
{
    public class ScheduleState
    {
        public List<Halt> Passed { get; set; } = new List<Halt>();

        public List<Halt> Upcoming { get; set; } = new List<Halt>();

        public TimeSpan TimeLeft { get; set; } = TimeSpan.Zero;

        public bool IsTimeLeftAtStation { get; set; } = false;

        public int Delay { get; set; }
    }
}
