using BusSimulator.Core.Configuration;
using BusSimulator.Core.Data.Interfaces;

namespace BusSimulator.Core.Data
{
    public class JsonDataRepositoryTarget : JsonDataRepository, IDataRepositoryTarget
    {
        public JsonDataRepositoryTarget(string filename) : base(filename)
        {
        }

        public void Save(DataRepository dataRepository)
        {
            JsonApplicationConfiguration.Save(this.filename, dataRepository);
        }
    }
}
