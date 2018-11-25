using BusSimulator.Core.Models;

namespace BusSimulator.Ui.Logic.Models
{
    public class DrivingParameters
    {
        public int LineId { get; set; }

        public bool IsDrivingForward { get; set; }

        public int StartHaltIndex { get; set; }

        public Time StartRunTime { get; set; }

        public Time StartTime { get; set; }
    }
}
