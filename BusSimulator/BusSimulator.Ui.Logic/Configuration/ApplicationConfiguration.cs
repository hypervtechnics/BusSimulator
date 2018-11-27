using BusSimulator.Core.Configuration;
using BusSimulator.Core.Models;

namespace BusSimulator.Ui.Logic.Configuration
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            this.JsonDataConfiguration = new JsonDataConfiguration();

            this.Overlay = new OverlayConfiguration();
            this.KeyBinding = new KeyBindingConfiguration();

            this.SimulationBehaviour = new SimulationBehaviourOptions();
            this.SimulationDisplay = new SimulationDisplayOptions();
        }

        public JsonDataConfiguration JsonDataConfiguration { get; set; }


        public OverlayConfiguration Overlay { get; set; }

        public KeyBindingConfiguration KeyBinding { get; set; }

        public SimulationBehaviourOptions SimulationBehaviour { get; set; }

        public SimulationDisplayOptions SimulationDisplay { get; set; }
    }
}
