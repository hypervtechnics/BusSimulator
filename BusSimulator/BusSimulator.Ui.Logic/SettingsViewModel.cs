using BusSimulator.Core.Configuration;
using BusSimulator.Ui.Logic.Configuration;
using BusSimulator.Ui.Logic.Messages;
using BusSimulator.Ui.Logic.Models;

using GalaSoft.MvvmLight.Command;

using System.Windows.Input;

namespace BusSimulator.Ui.Logic
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ApplicationConfiguration applicationConfiguration;

        public SettingsViewModel(ApplicationConfiguration applicationConfiguration)
        {
            this.applicationConfiguration = applicationConfiguration;

            this.InitializeCommands();
        }

        private void InitializeCommands()
        {
            this.KeyCommand = new RelayCommand<Key>((k) =>
            {
                this.LastKeyPressed = k.ToString().ToLower();
            });

            this.SaveCommand = new RelayCommand(() =>
            {
                JsonApplicationConfiguration.Save("Configuration", this.applicationConfiguration);

                //Broadcast settings update
                this.MessengerInstance.Send(new RefreshSettingsMessage());
            });

            this.SaveKeyCommand = new RelayCommand<string>((s) =>
            {
                switch (s)
                {
                    case "Pause":
                        this.KeyBindingPause = this.LastKeyPressed;
                        break;
                    case "Skip":
                        this.KeyBindingSkip = this.LastKeyPressed;
                        break;
                    case "Lock":
                        this.KeyBindingLock = this.LastKeyPressed;
                        break;
                }
            });

            this.AboutCommand = new RelayCommand(() =>
            {
                this.MessengerInstance.Send(new OpenAboutMessage());
            });
        }

        public string LastKeyPressed { get; private set; }

        public string KeyBindingSkip { get => this.applicationConfiguration.KeyBinding.SkipKey; set => this.applicationConfiguration.KeyBinding.SkipKey = value; }

        public string KeyBindingPause { get => this.applicationConfiguration.KeyBinding.PauseKey; set => this.applicationConfiguration.KeyBinding.PauseKey = value; }

        public string KeyBindingLock { get => this.applicationConfiguration.KeyBinding.LockKey; set => this.applicationConfiguration.KeyBinding.LockKey = value; }

        public MatrixColorType MatrixColor { get => this.applicationConfiguration.Overlay.MatrixColor; set => this.applicationConfiguration.Overlay.MatrixColor = value; }

        public bool ShowFrameCounter { get => this.applicationConfiguration.Overlay.ShowFrameCalculationTime; set => this.applicationConfiguration.Overlay.ShowFrameCalculationTime = value; }

        public string OverlayNotInServiceDestination { get => this.applicationConfiguration.Overlay.NotInServiceDestination; set => this.applicationConfiguration.Overlay.NotInServiceDestination = value; }

        public string OverlayNotInServiceRunningText { get => this.applicationConfiguration.Overlay.NotInServiceText; set => this.applicationConfiguration.Overlay.NotInServiceText = value; }

        public string OverlayNotInServiceSymbol { get => this.applicationConfiguration.Overlay.NotInServiceSymbol; set => this.applicationConfiguration.Overlay.NotInServiceSymbol = value; }

        public string SimulationDisplayStopDelimiter { get => this.applicationConfiguration.SimulationDisplay.TextStopDelimiter; set => this.applicationConfiguration.SimulationDisplay.TextStopDelimiter = value; }

        public string SimulationDisplayComingFrom { get => this.applicationConfiguration.SimulationDisplay.TextComingFrom; set => this.applicationConfiguration.SimulationDisplay.TextComingFrom = value; }

        #region Commands
        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand<string> SaveKeyCommand { get; private set; }

        public RelayCommand<Key> KeyCommand { get; private set; }

        public RelayCommand AboutCommand { get; private set; }
        #endregion
    }
}
