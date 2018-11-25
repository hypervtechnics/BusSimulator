using BusSimulator.Ui.Logic.Models;

namespace BusSimulator.Ui.Logic.Messages
{
    public class StartDrivingMessage
    {
        public StartDrivingMessage(DrivingParameters parameters)
        {
            this.Parameters = parameters;
        }

        public DrivingParameters Parameters { get; set; }
    }
}
