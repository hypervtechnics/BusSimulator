using BusSimulator.Core.Configuration;
using BusSimulator.Core.Data;
using BusSimulator.Core.Data.Interfaces;
using BusSimulator.Core.DataAccess;
using BusSimulator.Core.DataAccess.Interfaces;
using BusSimulator.Core.Logic;
using BusSimulator.Core.Logic.Abstractions;
using BusSimulator.Ui.Logic.Configuration;

using Ninject;
using Ninject.Modules;

namespace BusSimulator.Ui.Logic
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
            //Bind base configuration and data repository
            //this.Bind<IDataRepositorySource>().To<DesignDataSource>().InSingletonScope();
            this.Bind<IDataRepositorySource>().To<JsonDataRepositorySource>().InSingletonScope();

            this.Bind<IDataRepositoryTarget>().To<JsonDataRepositoryTarget>().InSingletonScope();
            this.Bind<IDataRepository>().ToMethod<DataRepository>(c => DataRepository.Load(c.Kernel.Get<IDataRepositorySource>())).InSingletonScope();

            //Bind data access
            this.Bind<ILineDataAccess>().To<LineDataAccess>().InSingletonScope();
            this.Bind<IStopDataAccess>().To<StopDataAccess>().InSingletonScope();

            //Bind simulation related components
            this.Bind<IScheduleService>().To<ScheduleService>().InSingletonScope();
            this.Bind<IStopDisplayService>().To<StopDisplayService>().InSingletonScope();
            this.Bind<ISimulationFactory>().To<SimulationFactory>().InSingletonScope();

            //Bind configuration
            this.Bind<ApplicationConfiguration>().ToMethod((c) => JsonApplicationConfiguration.Load<ApplicationConfiguration>("Configuration")).InSingletonScope();

            //Bind view models
            this.Bind<MainViewModel>().ToSelf();
            this.Bind<DriveViewModel>().ToSelf();
            this.Bind<AboutViewModel>().ToSelf();
            this.Bind<SettingsViewModel>().ToSelf();
            this.Bind<LineManagementViewModel>().ToSelf();
            this.Bind<StopManagementViewModel>().ToSelf();
        }
    }
}
