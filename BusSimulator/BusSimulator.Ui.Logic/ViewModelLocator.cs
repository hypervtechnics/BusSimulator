
using DryIoc;

namespace BusSimulator.Ui.Logic
{
    public class ViewModelLocator
    {
        private readonly IContainer container;

        public ViewModelLocator()
        {
            this.container = new Container();
            IocModule.Load(this.container);
        }

        public AboutViewModel About
        {
            get
            {
                return this.container.Resolve<AboutViewModel>();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return this.container.Resolve<SettingsViewModel>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return this.container.Resolve<MainViewModel>();
            }
        }

        public LineManagementViewModel LineManagement
        {
            get
            {
                return this.container.Resolve<LineManagementViewModel>();
            }
        }

        public StopManagementViewModel StopManagement
        {
            get
            {
                return this.container.Resolve<StopManagementViewModel>();
            }
        }

        public DriveViewModel Drive
        {
            get
            {
                return this.container.Resolve<DriveViewModel>();
            }
        }
    }
}
