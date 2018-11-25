using BusSimulator.Core.Models;

namespace BusSimulator.Core.Logic.Abstractions
{
    public interface IStopDisplayService
    {
        StopDisplayState GetStopDisplay(Time time, int stopId);
    }
}
