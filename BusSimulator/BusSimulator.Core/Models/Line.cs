using BusSimulator.Core.Data;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;

namespace BusSimulator.Core.Models
{
    public class Line : RepositoryObject
    {
        public string Symbol { get; set; }

        public bool CanHoldBendyBuses { get; set; }

        public bool IsFastBus { get; set; }

        public TimeType ServiceTime { get; set; }

        public List<Time> ForwardTimes { get; set; } = new List<Time>();

        public List<Time> BackwardTimes { get; set; } = new List<Time>();

        public List<ScheduleItem> ForwardSchedule { get; set; } = new List<ScheduleItem>();

        public List<ScheduleItem> BackwardSchedule { get; set; } = new List<ScheduleItem>();

        [JsonIgnore]
        public string LineSummary
        {
            get
            {
                //Origin and destination
                (Stop origin, Stop destination) GetRoute(List<ScheduleItem> schedule)
                {
                    return (schedule.FirstOrDefault()?.Stop, schedule.LastOrDefault()?.Stop);
                }

                var (forwardOrigin, forwardDestination) = GetRoute(this.ForwardSchedule);
                var (backwardOrigin, backwardDestination) = GetRoute(this.ForwardSchedule);

                var forwardString = string.Empty;
                if (forwardOrigin != null && forwardDestination != null && this.ForwardSchedule.Count > 1)
                {
                    forwardString = forwardOrigin.Name + " ⇨ " + forwardDestination.Name;
                }

                var backwardString = string.Empty;
                if (backwardOrigin != null && backwardDestination != null && this.BackwardSchedule.Count > 1)
                {
                    backwardString = backwardOrigin + " ⇨ " + backwardDestination;
                }

                var finalDirectionString = string.Empty;
                if (forwardString != string.Empty && backwardString != string.Empty)
                {
                    if ((forwardDestination.Id == backwardOrigin.Id && forwardOrigin.Id == backwardDestination.Id)
                        || (forwardDestination.Id == backwardDestination.Id && forwardOrigin.Id == backwardOrigin.Id))
                    {
                        finalDirectionString = forwardOrigin.Name + " ⬄ " + forwardDestination.Name;
                    }
                    else
                    {
                        finalDirectionString = forwardString + " & " + backwardString;
                    }
                }
                else if (forwardString != string.Empty && backwardString == string.Empty)
                {
                    finalDirectionString = forwardString;
                }
                else
                {
                    finalDirectionString = backwardString;
                }

                if (!string.IsNullOrEmpty(finalDirectionString))
                {
                    finalDirectionString = " (" + finalDirectionString + ")";
                }

                return this.Symbol + finalDirectionString;
            }
        }

        public override void CopyTo(RepositoryObject other)
        {
            if (other is Line otherLine)
            {
                otherLine.CanHoldBendyBuses = this.CanHoldBendyBuses;
                otherLine.ForwardTimes = this.ForwardTimes;
                otherLine.ForwardSchedule = this.ForwardSchedule;
                otherLine.BackwardTimes = this.BackwardTimes;
                otherLine.BackwardSchedule = this.BackwardSchedule;
                otherLine.IsFastBus = this.IsFastBus;
                otherLine.ServiceTime = this.ServiceTime;
                otherLine.Symbol = this.Symbol;
            }
        }
    }
}
