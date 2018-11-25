using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Extensions;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BusSimulator.Core.Logic
{
    public class Simulation : ISimulation
    {
        private readonly ILineDataAccess lineDa;
        private readonly IScheduleService scheduleService;
        private readonly SimulationOptions options;
        private readonly List<Halt> halts;

        private int nextHalt;
        private bool wasLockedAtPreviousTick;

        public Simulation(ILineDataAccess lineDa, IScheduleService scheduleService, SimulationOptions options)
        {
            this.lineDa = lineDa;
            this.scheduleService = scheduleService;
            this.options = options;
            this.halts = new List<Halt>();
            this.Line = this.lineDa.GetById(options.LineId);

            this.IsLockedToNext = false;
            this.wasLockedAtPreviousTick = false;

            this.Initialize();
        }

        public SimulationState State { get; private set; }

        public Line Line { get; }

        public bool IsLockedToNext { get; set; }

        #region Initialization methods
        private void Initialize()
        {
            //Create a state
            this.State = new SimulationState(this.options.StartTime);

            this.InitializeSchedule();
        }

        private void InitializeSchedule()
        {
            //Get internal halts to track delay by
            this.halts.AddRange(this.scheduleService.GetHalts(this.Line, this.options.IsDrivingForward, this.options.StartRunTime));

            //Set internal state for next stop
            this.nextHalt = this.options.StartHaltIndex;

            //Perform one iteration to determine a potential delay and fill the schedule state
            this.ProcessTick();
        }
        #endregion

        #region Processing
        public void Tick(bool requestStopJump)
        {
            bool passedStopAccordingToSchedule = false;

            //Check if the user passed a stop according to schedule only if not locked
            if (!this.IsLockedToNext)
            {
                var nextStop = this.State.Schedule.Upcoming.FirstOrDefault();
                if (nextStop != null)
                {
                    passedStopAccordingToSchedule = nextStop.DepartureTime < this.State.Time;
                }
            }

            this.InternalTick((((this.wasLockedAtPreviousTick && passedStopAccordingToSchedule) || passedStopAccordingToSchedule) && !this.IsLockedToNext) || requestStopJump);
            this.wasLockedAtPreviousTick = this.IsLockedToNext;
        }

        private void InternalTick(bool didPassStop)
        {
            //Move stop one up
            if (didPassStop)
            {
                this.PassStop();
            }

            this.ProcessTick();

            //Perform tick
            this.State.Time.TotalSeconds++;
        }

        private void PassStop()
        {
            int newHaltIndex = this.nextHalt + 1;

            if (this.halts.Count >= newHaltIndex)
            {
                this.nextHalt = newHaltIndex;
            }
        }

        private void ProcessTick()
        {
            //Update the schedule state halts and delay
            this.UpdateState();

            //Update the display if there are changes to e.g. route finished
            this.UpdateDisplay();

            //Update the left time
            this.UpdateLeftTime();
        }

        private void UpdateState()
        {
            this.State.Schedule.Passed.Clear();
            this.State.Schedule.Passed = this.halts.Take(this.nextHalt).ToList();

            this.State.Schedule.Upcoming.Clear();
            this.State.Schedule.Upcoming = this.halts.Skip(this.nextHalt).ToList();

            this.State.Schedule.Delay = this.GetDelay();
        }

        private int GetDelay()
        {
            var nextHalt = this.State.Schedule.Upcoming.FirstOrDefault();

            if (nextHalt == null)
            {
                return 0;
            }
            else
            {
                if (nextHalt.ArrivalTime <= this.State.Time)
                {
                    return (int) (this.State.Time - nextHalt.ArrivalTime).TotalMinutes;
                }
                else
                {
                    var previousHalt = this.State.Schedule.Passed.LastOrDefault();

                    if (previousHalt == null)
                    {
                        return 0;
                    }
                    else
                    {
                        if (previousHalt.DepartureTime > this.State.Time)
                        {
                            return (int) (previousHalt.DepartureTime - this.State.Time).TotalMinutes * -1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        private void UpdateLeftTime()
        {
            var nextStop = this.State.Schedule.Upcoming.FirstOrDefault();

            if (nextStop == null)
            {
                this.State.Schedule.TimeLeft = TimeSpan.Zero;
            }
            else
            {
                var realTime = nextStop.ArrivalTime + this.State.Schedule.Delay;

                //Normally the time left is referenced to the arrival time
                //If the arrival time has already passed we take the departure time to make some kind of "How long may I rest?" or "Oh damn how much time left to at least depart in time?"
                //Indicate in status to make visualization possible and also set to false to react to changes between ticks
                if (this.State.Time >= realTime)
                {
                    realTime = nextStop.DepartureTime + this.State.Schedule.Delay;
                    this.State.Schedule.IsTimeLeftAtStation = true;
                }
                else
                {
                    this.State.Schedule.IsTimeLeftAtStation = false;
                }

                this.State.Schedule.TimeLeft = realTime - this.State.Time;
            }
        }

        private void UpdateDisplay()
        {
            if (this.State.Schedule.Upcoming.Count > 0)
            {
                this.State.Display.Destination = this.halts.Last().Stop.Name;
                this.State.Display.Symbol = this.Line.Symbol;
                this.State.Display.IsOutOfService = false;

                //Some cool display formatting based on some conditions
                if (this.State.Schedule.Upcoming.Count == 1)
                {
                    this.State.Display.RunningText = string.Format(this.options.Display.TextComingFrom, this.halts[0].Stop.Name);
                }
                else
                {
                    var stopsOnDisplay = this.halts.Skip(this.State.Schedule.Passed.Count).Where(h => h.Stop.IsMajor).DropLast(1).Select(h => h.Stop.Name).ToList();

                    if (stopsOnDisplay.Count == 0)
                    {
                        stopsOnDisplay = this.halts.Skip(this.State.Schedule.Passed.Count).DropLast(1).Select(h => h.Stop.Name).ToList();
                    }

                    this.State.Display.RunningText = string.Join(this.options.Display.TextStopDelimiter, stopsOnDisplay);
                }
            }
            else
            {
                this.State.Display.IsOutOfService = true;
            }
        }
        #endregion
    }
}
