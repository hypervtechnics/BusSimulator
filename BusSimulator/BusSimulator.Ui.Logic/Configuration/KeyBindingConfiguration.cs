namespace BusSimulator.Ui.Logic.Configuration
{
    public class KeyBindingConfiguration
    {
        public KeyBindingConfiguration()
        {
            this.LockKey = "l";
            this.SkipKey = "oemquestion";
            this.PauseKey = "p";
        }

        public string LockKey { get; set; }

        public string SkipKey { get; set; }

        public string PauseKey { get; set; }
    }
}
