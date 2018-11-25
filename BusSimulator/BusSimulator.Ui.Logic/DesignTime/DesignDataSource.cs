using BusSimulator.Core.Data;
using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.Models;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BusSimulator.Ui.Logic.DesignTime
{
    public class DesignDataSource : IDataRepositorySource
    {
        public DataRepository Load()
        {
            Debug.WriteLine("[Data] Mocking data repository");

            return new DataRepository()
            {
                Stops = new List<Stop>()
                {
                    new Stop()
                    {
                        IsMajor = true,
                        Name = "ZOB",
                        Id = 1
                    },
                    new Stop()
                    {
                        IsMajor = true,
                        Name = "Main station",
                        Id = 2
                    },
                    new Stop()
                    {
                        IsMajor = false,
                        Name = "Siegwalden Lido",
                        Id = 3
                    },
                    new Stop()
                    {
                        IsMajor = true,
                        Name = "Astra Park",
                        Id = 4
                    },
                    new Stop()
                    {
                        IsMajor = false,
                        Name = "Freefield Siedlung",
                        Id = 5
                    }
                },
                Lines = new List<Line>()
                {
                    new Line()
                    {
                        Id = 1,
                        Symbol = "M1",
                        CanHoldBendyBuses = true,
                        IsFastBus = true,
                        ServiceTime = TimeType.Normal,
                        ForwardSchedule = new List<ScheduleItem>()
                        {
                            new ScheduleItem()
                            {
                                StopId = 1,
                                RestTime = 0,
                                AfterTravelForTime = 3
                            },
                            new ScheduleItem()
                            {
                                StopId = 2,
                                RestTime = 1,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 3,
                                RestTime = 0,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 4,
                                RestTime = 0,
                                AfterTravelForTime = 0
                            }
                        },
                        ForwardTimes = Time.Frequence(new Time((10 * 60 * 60) + (21 * 60)), 30, 10).ToList(),
                        BackwardSchedule = new List<ScheduleItem>()
                        {
                            new ScheduleItem()
                            {
                                StopId = 4,
                                RestTime = 0,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 3,
                                RestTime = 0,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 2,
                                RestTime = 1,
                                AfterTravelForTime = 3
                            },
                            new ScheduleItem()
                            {
                                StopId = 1,
                                RestTime = 0,
                                AfterTravelForTime = 0
                            }
                        },
                        BackwardTimes = Time.Frequence(new Time((10 * 60 * 60) + (45 * 60)), 30, 10).ToList()
                    },
                    new Line()
                    {
                        Id = 2,
                        Symbol = "L2",
                        CanHoldBendyBuses = true,
                        IsFastBus = true,
                        ServiceTime = TimeType.Special,
                        ForwardSchedule = new List<ScheduleItem>()
                        {
                            new ScheduleItem()
                            {
                                StopId = 1,
                                RestTime = 0,
                                AfterTravelForTime = 3
                            },
                            new ScheduleItem()
                            {
                                StopId = 5,
                                RestTime = 1,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 2,
                                RestTime = 1,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 3,
                                RestTime = 0,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 4,
                                RestTime = 0,
                                AfterTravelForTime = 0
                            }
                        },
                        ForwardTimes = Time.Frequence(new Time((06 * 60 * 60) + (00 * 60)), 30, 10).ToList(),
                        BackwardSchedule = new List<ScheduleItem>()
                        {
                            new ScheduleItem()
                            {
                                StopId = 4,
                                RestTime = 0,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 3,
                                RestTime = 0,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 2,
                                RestTime = 1,
                                AfterTravelForTime = 4
                            },
                            new ScheduleItem()
                            {
                                StopId = 5,
                                RestTime = 2,
                                AfterTravelForTime = 3
                            },
                            new ScheduleItem()
                            {
                                StopId = 1,
                                RestTime = 0,
                                AfterTravelForTime = 0
                            }
                        },
                        BackwardTimes = Time.Frequence(new Time((06 * 60 * 60) + (30 * 60)), 30, 10).ToList()
                    }
                }
            };
        }
    }
}
