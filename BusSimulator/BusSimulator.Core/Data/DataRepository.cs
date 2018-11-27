using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.Models;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace BusSimulator.Core.Data
{
    public class DataRepository : IDataRepository
    {
        [JsonIgnore]
        public IDataRepositoryTarget Target { get; set; }

        public List<Line> Lines { get; set; } = new List<Line>();

        public List<Stop> Stops { get; set; } = new List<Stop>();

        public static DataRepository Load(IDataRepositorySource source)
        {
            return source.Load();
        }

        public void Save()
        {
            this.Target?.Save(this);
        }
    }
}
