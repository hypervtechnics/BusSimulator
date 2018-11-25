using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Models;

using System.Collections.Generic;
using System.Linq;

namespace BusSimulator.Core.DataAccess
{
    public class LineDataAccess : BaseDataAccess<Line>, ILineDataAccess
    {
        private readonly StopDataAccess stopDa;

        public LineDataAccess(IDataRepository repository, StopDataAccess stopDa) : base(repository, repo => repo.Lines)
        {
            this.stopDa = stopDa;
        }

        public Dictionary<Line, LineDirection> GetLinesStoppingAt(int stopId)
        {
            var stoppingLines = new Dictionary<Line, LineDirection>();

            foreach (var line in this.GetAll())
            {
                var isBackward = line.BackwardSchedule.Any(s => s.StopId == stopId);
                var isForward = line.ForwardSchedule.Any(s => s.StopId == stopId);

                if (isBackward || isForward)
                {
                    LineDirection direction = LineDirection.Both;

                    if (isForward != isBackward)
                    {
                        direction = isForward ? LineDirection.Forward : LineDirection.Backward;
                    }

                    stoppingLines.Add(line, direction);
                }
            }

            return stoppingLines;
        }

        protected override Line ResolveComplexTypes(Line obj)
        {
            if (obj != null)
            {
                //Use local function
                void ResolveStops(List<ScheduleItem> list)
                {
                    foreach (var item in list)
                    {
                        item.Stop = this.stopDa.GetById(item.StopId);
                    }
                }

                ResolveStops(obj.BackwardSchedule);
                ResolveStops(obj.ForwardSchedule);
            }

            return obj;
        }
    }
}
