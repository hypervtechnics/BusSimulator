using BusSimulator.Ui.Logic;
using BusSimulator.Ui.Logic.Messages;
using BusSimulator.Ui.Logic.Utils;

using GalaSoft.MvvmLight.Messaging;

using System.Windows;

namespace BusSimulator.Ui
{
    public partial class SettingsWindow : Window
    {
        private KeyboardListener keyboardListener;

        public SettingsWindow()
        {
            this.InitializeComponent();

            this.InitMessenger();
        }

        private void InitMessenger()
        {
            Messenger.Default.Register<OpenAboutMessage>(this, msg =>
            {
                new AboutWindow().ShowDialog();
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.keyboardListener = new KeyboardListener();
            this.keyboardListener.KeyDown += this.KeyboardListener_KeyDown;
        }

        private void KeyboardListener_KeyDown(object sender, RawKeyEventArgs e)
        {
            if (this.DataContext is SettingsViewModel vm)
            {
                vm.KeyCommand.Execute(e.Key);
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.keyboardListener.Dispose();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataContext is SettingsViewModel vm)
            {
                vm.SaveCommand.Execute(null);
            }
        }
    }
}
