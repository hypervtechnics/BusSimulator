
using Ninject;

namespace BusSimulator.Ui.Logic
{
    public class ViewModelLocator
    {
        private readonly IKernel kernel;

        public ViewModelLocator()
        {
            this.kernel = new StandardKernel(new IocModule());
        }

        public AboutViewModel About
        {
            get
            {
                return this.kernel.Get<AboutViewModel>();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return this.kernel.Get<SettingsViewModel>();
            }
        }

        public MainViewModel Main
        {
            get
            {
                return this.kernel.Get<MainViewModel>();
            }
        }

        public LineManagementViewModel LineManagement
        {
            get
            {
                return this.kernel.Get<LineManagementViewModel>();
            }
        }

        public StopManagementViewModel StopManagement
        {
            get
            {
                return this.kernel.Get<StopManagementViewModel>();
            }
        }

        public DriveViewModel Drive
        {
            get
            {
                return this.kernel.Get<DriveViewModel>();
            }
        }
    }
}
