namespace BusSimulator.Core.Models
{
    public class Halt
    {
        public Stop Stop { get; set; }

        public Time DepartureTime { get; set; }

        public Time ArrivalTime { get; set; }
    }
}
