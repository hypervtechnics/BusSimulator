
using BusSimulator.Ui.Logic;
using BusSimulator.Ui.Logic.Utils;

using System.Diagnostics;
using System.Windows;

namespace BusSimulator.Ui
{
    public partial class DriveWindow : Window
    {
        private KeyboardListener keyboardListener;

        public DriveWindow()
        {
            this.InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.keyboardListener = new KeyboardListener();
            this.keyboardListener.KeyDown += this.KeyboardListener_KeyDown;

            this.PutCenterRight();
        }

        private void KeyboardListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            Debug.WriteLine("[Keyboard] " + args.Key.ToString());

            if (this.DataContext is DriveViewModel vm)
            {
                vm.KeyCommand.Execute(args.Key);
            }
        }

        private void PutCenterRight()
        {
            const int xOffset = 45;
            const int yOffset = 40;

            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - xOffset;
            this.Top = yOffset;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.keyboardListener.Dispose();
        }
    }
}
