
using Newtonsoft.Json;

namespace BusSimulator.Core.Models
{
    public class ScheduleItem
    {
        public int RestTime { get; set; }

        public int AfterTravelForTime { get; set; }

        [JsonIgnore]
        public Stop Stop { get; set; }

        public int StopId { get; set; }
    }
}