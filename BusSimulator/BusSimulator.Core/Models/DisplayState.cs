namespace BusSimulator.Core.Models
{
    public class DisplayState
    {
        public string Symbol { get; set; }

        public string Destination { get; set; }

        public string RunningText { get; set; }

        public bool IsOutOfService { get; set; }
    }
}
