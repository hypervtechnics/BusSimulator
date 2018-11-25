using BusSimulator.Ui.Logic.Models;

namespace BusSimulator.Ui.Logic.Configuration
{
    public class OverlayConfiguration
    {
        public OverlayConfiguration()
        {
            this.ShowFrameCalculationTime = true;
            this.MatrixColor = MatrixColorType.Green;

            this.NotInServiceSymbol = "0";
            this.NotInServiceDestination = "Not in service";
            this.NotInServiceText = "";
        }

        public bool ShowFrameCalculationTime { get; set; }

        public MatrixColorType MatrixColor { get; set; }

        public string NotInServiceSymbol { get; set; }

        public string NotInServiceDestination { get; set; }

        public string NotInServiceText { get; set; }
    }
}
