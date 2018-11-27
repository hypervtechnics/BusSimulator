using BusSimulator.Core.Configuration;
using BusSimulator.Core.Data;
using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.DataAccess;
using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Logic;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Ui.Logic.Configuration;

using DryIoc;

namespace BusSimulator.Ui.Logic
{
    public static class IocModule
    {
        public static void Load(IContainer c)
        {
            //Bind base configuration and data repository
            //c.Register<IDataRepositorySource, DesignDataSource>(reuse: Reuse.Singleton);
            c.Register<IDataRepositorySource, JsonDataRepositorySource>(reuse: Reuse.Singleton);
            c.Register<IDataRepositoryTarget, JsonDataRepositoryTarget>(reuse: Reuse.Singleton);
            c.RegisterDelegate<IDataRepository>(r => r.Resolve<IDataRepositorySource>().Load(), reuse: Reuse.Singleton);

            //Bind data access
            c.Register<IStopDataAccess, StopDataAccess>(reuse: Reuse.Singleton);
            c.Register<ILineDataAccess, LineDataAccess>(reuse: Reuse.Singleton);

            //Bind simulation related components
            c.Register<IScheduleService, ScheduleService>(reuse: Reuse.Singleton);
            c.Register<IStopDisplayService, StopDisplayService>(reuse: Reuse.Singleton);
            c.Register<ISimulationFactory, SimulationFactory>(reuse: Reuse.Singleton);

            //Bind configuration
            c.RegisterDelegate<ApplicationConfiguration>(r => JsonApplicationConfiguration.Load<ApplicationConfiguration>("Configuration"), reuse: Reuse.Singleton);
            c.RegisterDelegate<JsonDataConfiguration>(r => r.Resolve<ApplicationConfiguration>().JsonDataConfiguration); //Do this to be able to inject the options to the core assembly which does not know about application configuration class. Also it is transient to make it resolve each time to react to changing time stamps

            //Bind view models
            c.Register<MainViewModel>();
            c.Register<DriveViewModel>();
            c.Register<AboutViewModel>();
            c.Register<SettingsViewModel>();
            c.Register<LineManagementViewModel>();
            c.Register<StopManagementViewModel>();
        }
    }
}
