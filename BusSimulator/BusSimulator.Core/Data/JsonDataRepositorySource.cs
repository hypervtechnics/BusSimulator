using BusSimulator.Core.Configuration;
using BusSimulator.Core.Data.Interfaces;

namespace BusSimulator.Core.Data
{
    public class JsonDataRepositorySource : JsonDataRepository, IDataRepositorySource
    {
        public JsonDataRepositorySource() : base("Data")
        {
        }

        public DataRepository Load()
        {
            var loadedRepository = JsonApplicationConfiguration.Load<DataRepository>(this.filename);

            if (loadedRepository != null)
            {
                loadedRepository.Target = new JsonDataRepositoryTarget(this.filename);
            }

            return loadedRepository;
        }
    }
}
