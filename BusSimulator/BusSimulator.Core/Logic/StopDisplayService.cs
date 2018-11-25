using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Core.Models;

using System.Collections.Generic;
using System.Linq;

namespace BusSimulator.Core.Logic
{
    public class StopDisplayService : IStopDisplayService
    {
        private readonly ILineDataAccess lineDa;
        private readonly IScheduleService scheduleService;

        public StopDisplayService(ILineDataAccess lineDa, IScheduleService scheduleService)
        {
            this.lineDa = lineDa;
            this.scheduleService = scheduleService;
        }

        public StopDisplayState GetStopDisplay(Time time, int stopId)
        {
            var stoppingLines = this.lineDa.GetLinesStoppingAt(stopId);
            var result = new StopDisplayState();

            foreach (var line in stoppingLines)
            {
                List<List<Halt>> listOfHaltLists = new List<List<Halt>>();

                if (line.Value == LineDirection.Backward || line.Value == LineDirection.Both)
                {
                    listOfHaltLists.AddRange(this.GetHaltsForLineDirection(line.Key, false));
                }

                if (line.Value == LineDirection.Forward || line.Value == LineDirection.Both)
                {
                    listOfHaltLists.AddRange(this.GetHaltsForLineDirection(line.Key, true));
                }

                foreach (var haltList in listOfHaltLists)
                {
                    var possibleHalts = haltList
                        .Where(h => h.Stop.Id == stopId && (h.ArrivalTime >= time || h.DepartureTime >= time))
                        .ToList();

                    result.Events.AddRange(possibleHalts.Select(h => new StopDisplayEvent()
                    {
                        Destination = haltList.Last().Stop.Name,
                        Symbol = line.Key.Symbol,
                        ArrivalTime = h.ArrivalTime,
                        DepartureTime = h.DepartureTime
                    }));
                }
            }

            return result;
        }

        private List<List<Halt>> GetHaltsForLineDirection(Line line, bool forward)
        {
            List<Time> times;

            if (forward)
            {
                times = line.ForwardTimes;
            }
            else
            {
                times = line.BackwardTimes;
            }

            return times.Select(t => this.scheduleService.GetHalts(line, forward, t)).ToList();
        }
    }
}
