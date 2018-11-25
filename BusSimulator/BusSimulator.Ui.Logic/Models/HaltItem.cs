namespace BusSimulator.Ui.Logic.Models
{
    public class HaltItem
    {
        public string Name { get; set; }

        public bool IsMajor { get; set; }

        public string Arrival { get; set; }

        public string Departure { get; set; }

        public bool HasDifferentArrival { get => this.Arrival != this.Departure; }
    }
}
