using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Core.Models;

namespace BusSimulator.Core.Logic
{
    public class SimulationFactory : ISimulationFactory
    {
        private readonly ILineDataAccess lineDa;
        private readonly IScheduleService scheduleService;

        public SimulationFactory(ILineDataAccess lineDa, IScheduleService scheduleService)
        {
            this.lineDa = lineDa;
            this.scheduleService = scheduleService;
        }

        public ISimulation Init(SimulationOptions options)
        {
            return new Simulation(this.lineDa, this.scheduleService, options);
        }
    }
}
