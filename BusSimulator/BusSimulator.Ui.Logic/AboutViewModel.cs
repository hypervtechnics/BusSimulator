using System.Reflection;

namespace BusSimulator.Ui.Logic
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            this.AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public string AppVersion { get; }
    }
}
