namespace BusSimulator.Core.Models
{
    public class StopDisplayEvent
    {
        public string Symbol { get; set; }

        public string Destination { get; set; }

        public Time ArrivalTime { get; set; } = Time.Null;

        public Time DepartureTime { get; set; } = Time.Null;
    }
}
