using GalaSoft.MvvmLight;

using System.Diagnostics;

namespace BusSimulator.Ui.Logic
{
    public abstract class BaseViewModel : ViewModelBase
    {
        public static bool InDesign { get; } = IsInDesignMode();

        private new static bool IsInDesignMode()
        {
            return string.Equals("xdesproc", Process.GetCurrentProcess().ProcessName, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
