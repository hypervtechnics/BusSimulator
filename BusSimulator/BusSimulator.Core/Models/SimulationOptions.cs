namespace BusSimulator.Core.Models
{
    public class SimulationOptions
    {
        public int LineId { get; set; }

        public bool IsDrivingForward { get; set; }

        public int StartHaltIndex { get; set; }

        public Time StartRunTime { get; set; }

        public Time StartTime { get; set; }

        public SimulationBehaviourOptions Behaviour { get; set; } = new SimulationBehaviourOptions();

        public SimulationDisplayOptions Display { get; set; } = new SimulationDisplayOptions();
    }
}
