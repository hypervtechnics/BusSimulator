using BusSimulator.Core.Extensions;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Core.Models;
using BusSimulator.Ui.Logic.Configuration;
using BusSimulator.Ui.Logic.Messages;
using BusSimulator.Ui.Logic.Models;

using GalaSoft.MvvmLight.Command;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace BusSimulator.Ui.Logic
{
    public class DriveViewModel : BaseViewModel
    {
        private readonly ApplicationConfiguration options;
        private readonly DispatcherTimer timer;
        private readonly Stopwatch tickStopwatch;

        private readonly ISimulationFactory simulationFactory;
        private ISimulation simulation;
        private bool userDesiresStopJump = false;
        private bool forceHaltListUpdate = false;

        public DriveViewModel(ISimulationFactory simulationFactory, ApplicationConfiguration configuration)
        {
            //Accept injections
            this.simulationFactory = simulationFactory;
            this.options = configuration;

            //Init commands
            this.InitializeCommands();

            //Initialize stopwatch
            this.tickStopwatch = new Stopwatch();

            //Init dispatcher timer
            this.timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            this.timer.Tick += this.Dispatcher_Tick;

            //Init halts
            this.Halts = new ObservableCollection<HaltItem>();
            this.HaltsView = CollectionViewSource.GetDefaultView(this.Halts);

            //Activate pause
            this.IsPaused = true;

            //Accept refresh messages
            this.RegisterToMessenger();
        }

        private void InitializeCommands()
        {
            this.KeyCommand = new RelayCommand<Key>((k) =>
            {
                var key = k.ToString().ToLower();

                if (key == this.options.KeyBinding.LockKey.ToLower())
                {
                    //User pressed key to lock to current stop
                    //But only change lock state if game is not paused to prevent unwanted behaviour
                    if (!this.IsPaused)
                    {
                        this.IsLocked = !this.IsLocked;
                    }
                }
                else if (key == this.options.KeyBinding.SkipKey.ToLower())
                {
                    //User wants to skip a stop
                    //Activate the flag to be processed within next tick
                    //But only change lock state if game is not paused to prevent unwanted skips
                    if (!this.IsPaused)
                    {
                        this.userDesiresStopJump = true;
                    }
                }
                else if (key == this.options.KeyBinding.PauseKey.ToLower())
                {
                    //User wants to pause
                    this.IsPaused = !this.IsPaused;
                }
            });
        }

        private void RegisterToMessenger()
        {
            this.MessengerInstance.Register<RefreshDataMessage>(this, msg =>
            {
                if (!msg.Lines)
                {
                    this.forceHaltListUpdate = true;
                    this.TransferFromState();
                }
                else
                {
                    //TODO: Handle this way better
                    this.IsPaused = true;
                }
            });
            this.MessengerInstance.Register<RefreshSettingsMessage>(this, msg =>
            {
                this.RaisePropertyChanged(() => this.Options);
            });
        }

        public void Drive(DrivingParameters drivingParams)
        {
            //Prevent tick while refreshing
            this.IsPaused = true;

            //Init simulation
            this.simulation = this.simulationFactory.Init(new SimulationOptions()
            {
                Behaviour = this.options.SimulationBehaviour,
                Display = this.options.SimulationDisplay,
                IsDrivingForward = drivingParams.IsDrivingForward,
                StartHaltIndex = drivingParams.StartHaltIndex,
                LineId = drivingParams.LineId,
                StartRunTime = drivingParams.StartRunTime,
                StartTime = drivingParams.StartTime
            });

            //Force update of halt list to also reflect changes when changing direction or choosing route with same number of halts
            //This has to be done to save 2-7ms each frame spent on updating the list
            //The list only updates if the amount of halts changed
            this.forceHaltListUpdate = true;

            //Notify to show correct lock status on UI
            this.RaisePropertyChanged(nameof(this.IsLocked));

            //Reset overlay flags to do a fresh start
            this.userDesiresStopJump = false;

            //Set lock state accordingly to prevent the simulation from spinning down the stops if the user starts later than schedule
            this.IsLocked = drivingParams.StartTime >= drivingParams.StartRunTime;

            //Start everything and refresh the UI
            this.IsPaused = false;
        }

        private void Dispatcher_Tick(object sender, EventArgs e)
        {
            this.tickStopwatch.Restart();

            this.simulation.Tick(this.userDesiresStopJump);
            this.TransferFromState();

            //Reset stop jump flag
            this.userDesiresStopJump = false;

            this.tickStopwatch.Stop();
            if (this.options.Overlay.ShowFrameCalculationTime)
            {
                this.LastTickTime = this.tickStopwatch.ElapsedMilliseconds;
            }
        }

        private void TransferFromState()
        {
            //The left time has to contain the minutes and second precision
            //Also update the left time indicator
            this.TimeLeft = this.simulation.State.Schedule.TimeLeft.ToTimeString();
            this.IsLeftTimeAtStop = this.simulation.State.Schedule.IsTimeLeftAtStation;

            //The delay has to be led by a "+" if it is positive
            this.Delay = this.simulation.State.Schedule.Delay.ToString("+0;-#");

            //The current time including hour, minute and second
            this.Time = this.simulation.State.Time.ToString();

            //Set the appropiate line information. The values for a finished route are read from the global app configuration
            if (this.simulation.State.Display.IsOutOfService)
            {
                this.LineSymbol = this.options.Overlay.NotInServiceSymbol;
                this.LineDestination = this.options.Overlay.NotInServiceDestination;
                this.LineRunningText = this.options.Overlay.NotInServiceText;
            }
            else
            {
                this.LineSymbol = this.simulation.State.Display.Symbol;
                this.LineDestination = this.simulation.State.Display.Destination;
                this.LineRunningText = this.simulation.State.Display.RunningText;
            }

            //Update the halt list. But only if there is a different amount of halts. Clear it and add each item separete because there is no AddRange(range) method
            if (this.forceHaltListUpdate || this.Halts.Count != this.simulation.State.Schedule.Upcoming.Count)
            {
                this.forceHaltListUpdate = false;

                this.Halts.Clear();
                foreach (var halt in this.simulation.State.Schedule.Upcoming)
                {
                    this.Halts.Add(new HaltItem()
                    {
                        IsMajor = halt.Stop.IsMajor,
                        Name = halt.Stop.Name,
                        Arrival = halt.ArrivalTime.ToShortString(),
                        Departure = halt.DepartureTime.ToShortString()
                    });
                }
            }
        }

        public bool IsLeftTimeAtStop { get; private set; }

        public ApplicationConfiguration Options { get => this.options; }

        public long LastTickTime { get; private set; }

        public string TimeLeft { get; private set; }

        public bool IsPaused { get => !this.timer.IsEnabled; set => this.timer.IsEnabled = !value; }

        public bool IsLocked
        {
            get
            {
                return this.simulation == null || this.simulation.IsLockedToNext;
            }
            private set
            {
                if (this.simulation != null)
                {
                    this.simulation.IsLockedToNext = value;
                }
            }
        }

        private ObservableCollection<HaltItem> Halts { get; }

        public ICollectionView HaltsView { get; }

        public string LineSymbol { get; private set; }

        public string LineRunningText { get; private set; }

        public string LineDestination { get; private set; }

        public string Time { get; private set; }

        public string Delay { get; private set; }

        public RelayCommand<Key> KeyCommand { get; private set; }
    }
}
