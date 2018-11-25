using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Models;
using BusSimulator.Ui.Logic.Messages;

using GalaSoft.MvvmLight.Command;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace BusSimulator.Ui.Logic
{
    public class LineManagementViewModel : BaseViewModel
    {
        private readonly ILineDataAccess lineDataAccess;
        private readonly IStopDataAccess stopDataAccess;

        public LineManagementViewModel(ILineDataAccess lineDataAccess, IStopDataAccess stopDataAccess)
        {
            this.lineDataAccess = lineDataAccess;
            this.stopDataAccess = stopDataAccess;

            this.InitializeCommands();

            this.Lines = new ObservableCollection<Line>(this.lineDataAccess.GetAll());
            this.LinesView = CollectionViewSource.GetDefaultView(this.Lines);
            this.LinesView.CurrentChanged += this.LinesView_CurrentChanged;

            this.AvailableStops = new ObservableCollection<Stop>(this.stopDataAccess.GetAll());
            this.AvailableStopsView = CollectionViewSource.GetDefaultView(this.AvailableStops);

            this.ScheduleBackward = new ObservableCollection<ScheduleItem>();
            this.ScheduleBackwardView = CollectionViewSource.GetDefaultView(this.ScheduleBackward);

            this.ScheduleForward = new ObservableCollection<ScheduleItem>();
            this.ScheduleForwardView = CollectionViewSource.GetDefaultView(this.ScheduleForward);

            this.TimesForward = new ObservableCollection<Time>();
            this.TimesForwardView = CollectionViewSource.GetDefaultView(this.TimesForward);
            this.TimesForwardView.SortDescriptions.Add(new SortDescription(nameof(Time.TotalSeconds), ListSortDirection.Ascending));

            this.TimesBackward = new ObservableCollection<Time>();
            this.TimesBackwardView = CollectionViewSource.GetDefaultView(this.TimesBackward);
            this.TimesBackwardView.SortDescriptions.Add(new SortDescription(nameof(Time.TotalSeconds), ListSortDirection.Ascending));

            //Init default values
            this.IsRunForward = true;

            this.SelectedTime = new Time(0);

            this.FrequenceCount = 3;
            this.FrequenceInterval = 20;
            this.FrequenceStartTime = new Time(0);
        }

        private void LinesView_CurrentChanged(object sender, System.EventArgs e)
        {
            this.TimesBackward.Clear();
            this.TimesForward.Clear();

            this.ScheduleBackward.Clear();
            this.ScheduleForward.Clear();

            if (this.LinesView.CurrentItem is Line line)
            {
                void AddRangeToObservable<T>(ObservableCollection<T> ob, List<T> el)
                {
                    el.ForEach(elem => ob.Add(elem));
                }

                AddRangeToObservable(this.TimesBackward, line.BackwardTimes);
                AddRangeToObservable(this.TimesForward, line.ForwardTimes);

                AddRangeToObservable(this.ScheduleBackward, line.BackwardSchedule);
                AddRangeToObservable(this.ScheduleForward, line.ForwardSchedule);
            }

            this.LinesView.Refresh();
        }

        private void InitializeCommands()
        {
            this.AddLineCommand = new RelayCommand(() =>
            {
                var newLine = new Line() { Symbol = "000" };
                this.lineDataAccess.Add(newLine);

                this.Lines.Add(newLine);
                this.LinesView.MoveCurrentTo(newLine);
                this.RaisePropertyChanged(() => this.SelectedLine);

                this.MessengerInstance.Send(new RefreshDataMessage(true));
            });

            this.RemoveLineCommand = new RelayCommand(() =>
            {
                if (this.LinesView.CurrentItem is Line line)
                {
                    this.lineDataAccess.Delete(line);
                    this.Lines.Remove(line);

                    this.MessengerInstance.Send(new RefreshDataMessage(true));
                }
            });

            this.AddTimeCommand = new RelayCommand(() =>
            {
                if (this.SelectedTime != null)
                {
                    this.AddTime(this.SelectedTime);

                    this.lineDataAccess.Update(this.SelectedLine);

                    this.SelectedTime = new Time(0);
                }
            });

            this.AddTimeFrequenceCommand = new RelayCommand(() =>
            {
                if (this.FrequenceStartTime != null)
                {
                    var freq = Time
                        .Frequence(this.FrequenceStartTime, this.FrequenceInterval, this.FrequenceCount)
                        .Where(t => t.Overlap < 1)
                        .ToList();

                    freq.ForEach(t => this.AddTime(t));

                    this.FrequenceStartTime = new Time(freq.Count > 0 ? freq.Last().TotalSeconds : 0);
                }
            });

            this.RemoveTimeCommand = new RelayCommand(() =>
            {
                if (this.TimesView.CurrentItem is Time time && this.SelectedLine != null)
                {
                    if (this.IsRunForward)
                    {
                        this.SelectedLine.ForwardTimes.Remove(time);
                        this.TimesForward.Remove(time);
                    }
                    else
                    {
                        this.SelectedLine.BackwardTimes.Remove(time);
                        this.TimesBackward.Remove(time);
                    }

                    this.lineDataAccess.Update(this.SelectedLine);
                    this.LinesView.Refresh();
                }
            });

            this.AddScheduleCommand = new RelayCommand(() =>
            {
                if (this.AvailableStopsView.CurrentItem is Stop stop)
                {
                    var scheduleItem = new ScheduleItem()
                    {
                        Stop = stop,
                        StopId = stop.Id
                    };

                    if (this.IsRunForward)
                    {
                        this.SelectedLine.ForwardSchedule.Add(scheduleItem);
                        this.ScheduleForward.Add(scheduleItem);
                    }
                    else
                    {
                        this.SelectedLine.BackwardSchedule.Add(scheduleItem);
                        this.ScheduleBackward.Add(scheduleItem);
                    }

                    this.UpdateLine();
                }
            });

            this.RemoveScheduleCommand = new RelayCommand(() =>
            {
                if (this.ScheduleView.CurrentItem is ScheduleItem scheduleItem)
                {
                    if (this.IsRunForward)
                    {
                        this.SelectedLine.ForwardSchedule.Remove(scheduleItem);
                        this.ScheduleForward.Remove(scheduleItem);
                    }
                    else
                    {
                        this.SelectedLine.BackwardSchedule.Remove(scheduleItem);
                        this.ScheduleBackward.Remove(scheduleItem);
                    }

                    this.UpdateLine();
                }
            });

            this.MoveScheduleDownCommand = new RelayCommand(() =>
            {
                if (this.ScheduleView.CurrentItem is ScheduleItem scheduleItem)
                {
                    this.Move(scheduleItem, false);

                    this.UpdateLine();
                    this.ScheduleView.MoveCurrentTo(scheduleItem);
                }
            });

            this.MoveScheduleUpCommand = new RelayCommand(() =>
            {
                if (this.ScheduleView.CurrentItem is ScheduleItem scheduleItem)
                {
                    this.Move(scheduleItem, true);

                    this.UpdateLine();
                    this.ScheduleView.MoveCurrentTo(scheduleItem);
                }
            });

            this.ReverseCommand = new RelayCommand(() =>
            {
                if (this.SelectedLine != null)
                {
                    var sourceLineSchedule = this.IsRunForward ? this.SelectedLine.ForwardSchedule : this.SelectedLine.BackwardSchedule;
                    var targetSchedule = this.IsRunForward ? this.ScheduleBackward : this.ScheduleForward;
                    var targetLineSchedule = this.IsRunForward ? this.SelectedLine.BackwardSchedule : this.SelectedLine.ForwardSchedule;

                    if (sourceLineSchedule.Count > 1)
                    {
                        targetSchedule.Clear();
                        targetLineSchedule.Clear();

                        for (int i = sourceLineSchedule.Count - 1; i >= 0; i--)
                        {
                            int previousDrivingTime = 0;
                            int previousIndex = i - 1;

                            if (previousIndex >= 0)
                            {
                                previousDrivingTime = sourceLineSchedule[previousIndex].AfterTravelForTime;
                            }

                            var newSchedule = new ScheduleItem()
                            {
                                AfterTravelForTime = previousDrivingTime,
                                RestTime = sourceLineSchedule[i].RestTime,
                                Stop = sourceLineSchedule[i].Stop,
                                StopId = sourceLineSchedule[i].StopId
                            };

                            targetLineSchedule.Add(newSchedule);
                            targetSchedule.Add(newSchedule);
                        }

                        this.UpdateLine();
                    }
                }
            });
        }

        private void UpdateLine()
        {
            this.lineDataAccess.Update(this.SelectedLine);
            this.LinesView.Refresh();

            this.MessengerInstance.Send(new RefreshDataMessage(true));
        }

        private void Move(ScheduleItem item, bool up)
        {
            if (this.IsRunForward)
            {
                var currentIndex = this.SelectedLine.ForwardSchedule.IndexOf(item);
                var newIndex = currentIndex + (up ? -1 : 1);

                if (newIndex >= 0 && newIndex < this.SelectedLine.ForwardSchedule.Count)
                {
                    this.SelectedLine.ForwardSchedule.Remove(item);
                    this.ScheduleForward.Remove(item);

                    this.SelectedLine.ForwardSchedule.Insert(newIndex, item);
                    this.ScheduleForward.Insert(newIndex, item);
                }
            }
            else
            {
                var currentIndex = this.SelectedLine.BackwardSchedule.IndexOf(item);
                var newIndex = currentIndex + (up ? -1 : 1);

                if (newIndex >= 0 && newIndex < this.SelectedLine.BackwardSchedule.Count)
                {
                    this.SelectedLine.BackwardSchedule.Remove(item);
                    this.ScheduleBackward.Remove(item);

                    this.SelectedLine.BackwardSchedule.Insert(newIndex, item);
                    this.ScheduleBackward.Insert(newIndex, item);
                }
            }
        }

        private void AddTime(Time time)
        {
            if (this.SelectedLine != null)
            {
                if (this.IsRunForward)
                {
                    if (!this.SelectedLine.ForwardTimes.Any(t => t.TotalSeconds == time.TotalSeconds))
                    {
                        this.SelectedLine.ForwardTimes.Add(time);
                        this.TimesForward.Add(time);
                    }
                }
                else
                {
                    if (!this.SelectedLine.BackwardTimes.Any(t => t.TotalSeconds == time.TotalSeconds))
                    {
                        this.SelectedLine.BackwardTimes.Add(time);
                        this.TimesBackward.Add(time);
                    }
                }

                this.lineDataAccess.Update(this.SelectedLine);
            }
        }

        public Time FrequenceStartTime { get; set; }

        public int FrequenceInterval { get; set; }

        public int FrequenceCount { get; set; }

        public bool IsRunForward { get; set; }

        private ObservableCollection<ScheduleItem> ScheduleForward { get; }

        private ICollectionView ScheduleForwardView { get; }

        private ObservableCollection<ScheduleItem> ScheduleBackward { get; }

        private ICollectionView ScheduleBackwardView { get; }

        public ICollectionView ScheduleView { get => this.IsRunForward ? this.ScheduleForwardView : this.ScheduleBackwardView; }

        private ObservableCollection<Time> TimesBackward { get; }

        private ICollectionView TimesBackwardView { get; }

        private ObservableCollection<Time> TimesForward { get; }

        private ICollectionView TimesForwardView { get; }

        public ICollectionView TimesView { get => this.IsRunForward ? this.TimesForwardView : this.TimesBackwardView; }

        public Time SelectedTime { get; set; }

        private ObservableCollection<Stop> AvailableStops { get; }

        public ICollectionView AvailableStopsView { get; }

        private ObservableCollection<Line> Lines { get; }

        public ICollectionView LinesView { get; }

        public Line SelectedLine { get => this.LinesView.CurrentItem as Line; set => this.LinesView.MoveCurrentTo(value); }

        #region Commands
        public RelayCommand AddLineCommand { get; private set; }

        public RelayCommand RemoveLineCommand { get; private set; }

        public RelayCommand AddTimeCommand { get; private set; }

        public RelayCommand AddTimeFrequenceCommand { get; private set; }

        public RelayCommand RemoveTimeCommand { get; private set; }

        public RelayCommand AddScheduleCommand { get; private set; }

        public RelayCommand MoveScheduleUpCommand { get; private set; }

        public RelayCommand MoveScheduleDownCommand { get; private set; }

        public RelayCommand RemoveScheduleCommand { get; private set; }

        public RelayCommand ReverseCommand { get; private set; }
        #endregion
    }
}
