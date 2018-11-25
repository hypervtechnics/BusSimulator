using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Models;
using BusSimulator.Ui.Logic.Interfaces;

using GalaSoft.MvvmLight.Command;

using PropertyChanged;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;

namespace BusSimulator.Ui.Logic
{
    public class StopManagementViewModel : BaseViewModel
    {
        private readonly IStopDataAccess stopDataAccess;
        private readonly ILineDataAccess lineDataAccess;

        public StopManagementViewModel(IStopDataAccess stopDataAccess, ILineDataAccess lineDataAccess)
        {
            this.stopDataAccess = stopDataAccess;
            this.lineDataAccess = lineDataAccess;

            this.Stops = new ObservableCollection<Stop>(this.stopDataAccess.GetAll());
            this.StopsView = CollectionViewSource.GetDefaultView(this.Stops);

            this.InitializeCommands();
        }

        private void InitializeCommands()
        {
            this.AddCommand = new RelayCommand(() =>
            {
                var newStop = new Stop();
                this.stopDataAccess.Add(newStop);

                this.Stops.Add(newStop);
                this.StopsView.MoveCurrentTo(newStop);
            });

            this.RemoveCommand = new RelayCommand(() =>
            {
                if (this.StopsView.CurrentItem is Stop stop)
                {
                    this.Delete(stop);
                }
            });

            this.ImportCommand = new RelayCommand<Type>((dialogType) =>
            {
                if (Activator.CreateInstance(dialogType) is IDialogWindow dialog)
                {
                    (bool success, string filename) = dialog.ShowFileDialog(null, "Text|*.txt");

                    if (success && File.Exists(filename))
                    {
                        foreach (var stopFromFile in File.ReadAllLines(filename))
                        {
                            if (!this.Stops.Any(s => s.Name.Equals(stopFromFile, StringComparison.OrdinalIgnoreCase)))
                            {
                                var newStop = new Stop()
                                {
                                    Name = stopFromFile
                                };

                                this.stopDataAccess.Add(newStop);

                                this.Stops.Add(newStop);
                            }
                        }
                    }
                }
            });
        }

        private void Delete(Stop stop)
        {
            var lines = this.lineDataAccess.GetAll();

            foreach (var line in lines)
            {
                line.BackwardSchedule.RemoveAll(s => s.StopId == stop.Id);
                line.ForwardSchedule.RemoveAll(s => s.StopId == stop.Id);

                this.lineDataAccess.Update(line);
            }

            this.stopDataAccess.Delete(stop);
            this.Stops.Remove(stop);
        }

        private ObservableCollection<Stop> Stops { get; }

        public ICollectionView StopsView { get; }

        public Stop SelectedStop { get => this.StopsView.CurrentItem as Stop; set => this.StopsView.MoveCurrentTo(value); }

        [DependsOn(nameof(SelectedStop))]
        public List<Line> StoppingLines
        {
            get
            {
                if (this.StopsView.CurrentItem is Stop stop)
                {
                    return this.lineDataAccess.GetLinesStoppingAt(stop.Id)
                        .Select(kv => kv.Key)
                        .ToList();
                }
                else
                {
                    return new List<Line>();
                }
            }
        }

        #region Commands
        public RelayCommand AddCommand { get; private set; }

        public RelayCommand RemoveCommand { get; private set; }

        public RelayCommand<Type> ImportCommand { get; private set; }
        #endregion
    }
}
