using BusSimulator.Core.Models;

namespace BusSimulator.Core.Logic.Abstractions
{
    public interface ISimulation
    {
        SimulationState State { get; }

        Line Line { get; }

        bool IsLockedToNext { get; set; }

        void Tick(bool requestStopJump = false);
    }
}
