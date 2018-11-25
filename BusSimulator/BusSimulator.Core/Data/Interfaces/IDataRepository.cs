using BusSimulator.Core.Models;

using System.Collections.Generic;

namespace BusSimulator.Core.Data.Interfaces
{
    public interface IDataRepository
    {
        List<Line> Lines { get; set; }

        List<Stop> Stops { get; set; }

        IDataRepositoryTarget Target { get; set; }

        void Save();
    }
}
