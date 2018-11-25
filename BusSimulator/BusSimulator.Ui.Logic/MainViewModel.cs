using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Extensions;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Core.Models;
using BusSimulator.Ui.Logic.Messages;
using BusSimulator.Ui.Logic.Models;

using GalaSoft.MvvmLight.Command;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace BusSimulator.Ui.Logic
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ILineDataAccess lineDa;
        private readonly IScheduleService scheduleService;

        public MainViewModel(ILineDataAccess lineDa, IScheduleService scheduleService)
        {
            this.lineDa = lineDa;
            this.scheduleService = scheduleService;

            this.InitializeCommands();

            this.LineDirections = new ObservableCollection<LineDirectionItem>();
            this.LineDirectionsView = CollectionViewSource.GetDefaultView(this.LineDirections);
            this.LineDirectionsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(LineDirectionItem.Symbol)));
            this.LineDirectionsView.CurrentChanged += this.LineDirectionsView_CurrentChanged;

            this.PreviewHalts = new ObservableCollection<HaltItem>();
            this.PreviewHaltsView = CollectionViewSource.GetDefaultView(this.PreviewHalts);

            this.Times = new ObservableCollection<Time>();
            this.TimesView = CollectionViewSource.GetDefaultView(this.Times);
            this.TimesView.CurrentChanged += this.TimesView_CurrentChanged;

            this.EntryStops = new ObservableCollection<EntryStopItem>();
            this.EntryStopsView = CollectionViewSource.GetDefaultView(this.EntryStops);
            this.EntryStopsView.CurrentChanged += this.EntryStopsView_CurrentChanged;

            this.MessengerInstance.Register<RefreshDataMessage>(this, msg =>
            {
                if (msg.Lines)
                {
                    this.InitializeLines();
                }

                this.GeneratePreview();
            });

            this.InitializeLines();
        }

        #region Initialization
        private void InitializeCommands()
        {
            this.SettingsCommand = new RelayCommand(() =>
            {
                this.MessengerInstance.Send(new OpenSettingsMessage());
            });

            this.ManageLineCommand = new RelayCommand(() =>
            {
                this.MessengerInstance.Send(new OpenLineManagementMessage());
            });

            this.ManageStopCommand = new RelayCommand(() =>
            {
                this.MessengerInstance.Send(new OpenStopManagementMessage());
            });

            this.StartCommand = new RelayCommand(() =>
            {
                var ld = this.LineDirectionsView.CurrentItem as LineDirectionItem;
                var time = this.TimesView.CurrentItem as Time;
                var start = this.EntryStopsView.CurrentItem as EntryStopItem;

                var parameters = new DrivingParameters()
                {
                    IsDrivingForward = ld.IsForward,
                    LineId = ld.Line.Id,
                    StartHaltIndex = start.ScheduleIndex,
                    StartRunTime = time,
                    StartTime = new Time(this.SelectedStartTime)
                };

                this.MessengerInstance.Send(new StartDrivingMessage(parameters));
            }, () =>
            {
                return this.LineDirectionsView.CurrentItem != null
                && this.TimesView.CurrentItem != null
                && this.EntryStopsView.CurrentItem != null;
            });
        }

        private void InitializeLines()
        {
            this.LineDirections.Clear();

            foreach (var line in this.lineDa.GetAll())
            {
                if (line.ForwardSchedule.Count > 0)
                {
                    this.LineDirections.Add(new LineDirectionItem()
                    {
                        IsForward = true,
                        Destination = line.ForwardSchedule.Last().Stop,
                        Line = line
                    });
                }

                if (line.BackwardSchedule.Count > 0)
                {
                    this.LineDirections.Add(new LineDirectionItem()
                    {
                        IsForward = false,
                        Destination = line.BackwardSchedule.Last().Stop,
                        Line = line
                    });
                }
            }
        }
        #endregion

        private void GeneratePreview()
        {
            this.PreviewHalts.Clear();

            if (this.LineDirectionsView.CurrentItem is LineDirectionItem line && line != null
                && this.TimesView.CurrentItem is Time time && time != null
                && this.EntryStopsView.CurrentItem is EntryStopItem entry && entry != null)
            {
                foreach (var halt in this.scheduleService.GetHalts(line.Line, line.IsForward, time).Skip(entry.ScheduleIndex))
                {
                    this.PreviewHalts.Add(new HaltItem()
                    {
                        Arrival = halt.ArrivalTime.ToShortString(),
                        Departure = halt.DepartureTime.ToShortString(),
                        IsMajor = halt.Stop.IsMajor,
                        Name = halt.Stop.Name
                    });
                }
            }

            //Update the state of the start command
            this.StartCommand?.RaiseCanExecuteChanged();
        }

        #region UI Change Handling
        private void TimesView_CurrentChanged(object sender, EventArgs e)
        {
            if (this.TimesView.CurrentItem is Time time && time != null)
            {
                this.SelectedStartTime = DateTime.Now.Date.AddSeconds(time.TotalSeconds).AddMinutes(-2);
            }

            this.GeneratePreview();
        }

        private void LineDirectionsView_CurrentChanged(object sender, EventArgs e)
        {
            this.Times.Clear();
            this.EntryStops.Clear();

            if (this.LineDirectionsView.CurrentItem is LineDirectionItem line && line != null)
            {
                var schedule = this.scheduleService.GetSchedule(line.Line, line.IsForward).DropLast(1).ToList();
                for (int i = 0; i < schedule.Count; i++)
                {
                    this.EntryStops.Add(new EntryStopItem()
                    {
                        ScheduleIndex = i,
                        Stop = schedule[i].Stop
                    });
                }

                this.EntryStopsView.MoveCurrentTo(this.EntryStops.FirstOrDefault());

                foreach (var t in line.IsForward ? line.Line.ForwardTimes : line.Line.BackwardTimes)
                {
                    this.Times.Add(t);
                }

                this.TimesView.MoveCurrentTo(this.Times.FirstOrDefault());
            }
        }

        private void EntryStopsView_CurrentChanged(object sender, EventArgs e)
        {
            this.GeneratePreview();
        }
        #endregion

        #region Commands
        public RelayCommand StartCommand { get; private set; }

        public RelayCommand SettingsCommand { get; private set; }

        public RelayCommand ManageLineCommand { get; private set; }

        public RelayCommand ManageStopCommand { get; private set; }
        #endregion

        #region UI Binding
        #region Departure Times
        private ObservableCollection<Time> Times { get; }

        public ICollectionView TimesView { get; }
        #endregion

        #region Direction
        private ObservableCollection<LineDirectionItem> LineDirections { get; }

        public ICollectionView LineDirectionsView { get; }
        #endregion

        #region Halts
        private ObservableCollection<HaltItem> PreviewHalts { get; }

        public ICollectionView PreviewHaltsView { get; }
        #endregion

        #region Options
        public DateTime SelectedStartTime { get; set; }

        private ObservableCollection<EntryStopItem> EntryStops { get; }

        public ICollectionView EntryStopsView { get; set; }
        #endregion
        #endregion
    }
}
