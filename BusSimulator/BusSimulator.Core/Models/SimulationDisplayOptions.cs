namespace BusSimulator.Core.Models
{
    public class SimulationDisplayOptions
    {
        public SimulationDisplayOptions()
        {
            this.TextStopDelimiter = " > ";
            this.TextComingFrom = "Coming from {0}";
        }

        public string TextStopDelimiter { get; set; }

        public string TextComingFrom { get; set; }
    }
}