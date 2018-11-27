using BusSimulator.Core.Configuration;
using BusSimulator.Core.Data.Interfaces;

namespace BusSimulator.Core.Data
{
    public class JsonDataRepositoryTarget : IDataRepositoryTarget
    {
        private readonly JsonDataConfiguration jsonDataConfiguration;

        public JsonDataRepositoryTarget(JsonDataConfiguration jsonDataConfiguration)
        {
            this.jsonDataConfiguration = jsonDataConfiguration;
        }

        public void Save(DataRepository dataRepository)
        {
            JsonApplicationConfiguration.Save(this.jsonDataConfiguration.JsonDataRepositoryFile, dataRepository);
        }
    }
}
