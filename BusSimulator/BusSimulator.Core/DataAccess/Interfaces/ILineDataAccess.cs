using BusSimulator.Core.Models;

using System.Collections.Generic;

namespace BusSimulator.Core.DataAccess.Interfaces
{
    public interface ILineDataAccess : IDataAccess<Line>
    {
        Dictionary<Line, LineDirection> GetLinesStoppingAt(int stopId);
    }
}
