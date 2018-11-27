using BusSimulator.Core.Configuration;
using BusSimulator.Core.Data.Interfaces;

namespace BusSimulator.Core.Data
{
    public class JsonDataRepositorySource : IDataRepositorySource
    {
        private readonly JsonDataConfiguration jsonDataConfiguration;

        public JsonDataRepositorySource(JsonDataConfiguration jsonDataConfiguration)
        {
            this.jsonDataConfiguration = jsonDataConfiguration;
        }

        public DataRepository Load()
        {
            var loadedRepository = JsonApplicationConfiguration.Load<DataRepository>(this.jsonDataConfiguration.JsonDataRepositoryFile);

            if (loadedRepository != null)
            {
                loadedRepository.Target = new JsonDataRepositoryTarget(this.jsonDataConfiguration);
            }

            return loadedRepository;
        }
    }
}
