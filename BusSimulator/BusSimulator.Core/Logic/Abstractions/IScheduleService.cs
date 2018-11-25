using BusSimulator.Core.Models;

using System.Collections.Generic;

namespace BusSimulator.Core.Logic.Abstractions
{
    public interface IScheduleService
    {
        List<Halt> GetHalts(Line line, bool forward, Time time);

        List<ScheduleItem> GetSchedule(Line line, bool forward);
    }
}
