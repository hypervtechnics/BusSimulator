using System.Collections.Generic;

namespace BusSimulator.Core.Models
{
    public class StopDisplayState
    {
        public List<StopDisplayEvent> Events { get; set; } = new List<StopDisplayEvent>();
    }
}
