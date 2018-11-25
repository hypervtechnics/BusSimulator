using BusSimulator.Core.Models;

namespace BusSimulator.Ui.Logic.Models
{
    public class LineDirectionItem
    {
        public Line Line { get; set; }

        public bool IsForward { get; set; }

        public Stop Destination { get; set; }

        public string Symbol { get => this.Line.Symbol; }
    }
}
