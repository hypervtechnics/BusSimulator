namespace BusSimulator.Core.Models
{
    public class SimulationState
    {
        public SimulationState(Time time)
        {
            this.Time = time;
        }

        public ScheduleState Schedule { get; set; } = new ScheduleState();

        public DisplayState Display { get; set; } = new DisplayState();

        public Time Time { get; set; } = Time.Null;
    }
}
