using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Models;

namespace BusSimulator.Core.DataAccess
{
    public class StopDataAccess : BaseDataAccess<Stop>, IStopDataAccess
    {
        public StopDataAccess(IDataRepository repository) : base(repository, repo => repo.Stops)
        {
        }

        protected override Stop ResolveComplexTypes(Stop obj)
        {
            return obj;
        }
    }
}
