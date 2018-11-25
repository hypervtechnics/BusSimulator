using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Core.Models;

using System;
using System.Collections.Generic;

namespace BusSimulator.Core.Logic
{
    public class ScheduleService : IScheduleService
    {
        private readonly ILineDataAccess lineDa;

        public ScheduleService(ILineDataAccess lineDa)
        {
            this.lineDa = lineDa;
        }

        public List<Halt> GetHalts(Line line, bool forward, Time time)
        {
            var halts = new List<Halt>();
            var scheduleItems = this.GetSchedule(line, forward);
            var nextTime = time;

            foreach (var item in scheduleItems)
            {
                var halt = new Halt()
                {
                    Stop = item.Stop,
                    ArrivalTime = nextTime,
                    DepartureTime = nextTime + TimeSpan.FromMinutes(item.RestTime)
                };

                //This is to jump only to the next stop if the full minute passed
                halt.DepartureTime += TimeSpan.FromSeconds(59 - halt.DepartureTime.Second);

                halts.Add(halt);
                nextTime = new Time(halt.DepartureTime.TotalSeconds - halt.DepartureTime.Second) + TimeSpan.FromMinutes(item.AfterTravelForTime);
            }

            return halts;
        }

        public List<ScheduleItem> GetSchedule(Line line, bool forward)
        {
            if (forward)
            {
                return line.ForwardSchedule;
            }
            else
            {
                return line.BackwardSchedule;
            }
        }
    }
}
