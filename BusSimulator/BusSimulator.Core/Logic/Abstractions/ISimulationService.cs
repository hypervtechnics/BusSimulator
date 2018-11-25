using BusSimulator.Core.Models;

namespace BusSimulator.Core.Logic.Abstractions
{
    public interface ISimulationFactory
    {
        ISimulation Init(SimulationOptions options);
    }
}
