using BusSimulator.Core.Data;
using BusSimulator.Core.DataAccess;
using BusSimulator.Core.Models;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BusSimulator.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = DataRepository.Load(new JsonDataRepositorySource());

            var stda = new StopDataAccess(repo);
            var lida = new LineDataAccess(repo, stda);

            Debug.WriteLine("Layers have been created!");

            Stop stop = new Stop()
            {
                IsMajor = true,
                Name = "Main Station"
            };

            Stop stop2 = new Stop()
            {
                IsMajor = false,
                Name = "Sub station"
            };

            stda.Add(stop);
            stda.Add(stop2);

            Debug.WriteLine($"Stop 1: {stop.Id}\nStop 2: {stop2.Id}");

            ScheduleItem s1 = new ScheduleItem()
            {
                StopId = stop.Id,
                AfterTravelForTime = 5
            };

            ScheduleItem s2 = new ScheduleItem()
            {
                StopId = stop2.Id
            };

            Line line = new Line()
            {
                Symbol = "X1",
                IsFastBus = true,
                ServiceTime = TimeType.Normal,
                CanHoldBendyBuses = true,
                ForwardSchedule =
                {
                    s1,
                    s2
                },
                ForwardTimes = Time.Frequence(new Time(28800), 20, 6).ToList()
            };

            lida.Add(line);

            Console.WriteLine(lida.GetAll().Count);
            Console.ReadLine();

#if DEBUG
            File.Delete("data.json");
#endif
        }
    }
}
