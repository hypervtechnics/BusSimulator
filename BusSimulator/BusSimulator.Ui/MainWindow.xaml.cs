using BusSimulator.Ui.Logic;
using BusSimulator.Ui.Logic.Messages;

using GalaSoft.MvvmLight.Messaging;

using System.Windows;

namespace BusSimulator.Ui
{
    public partial class MainWindow : Window
    {
        private DriveWindow driveWindow;

        public MainWindow()
        {
            this.InitializeComponent();

            this.InitMessenger();
        }

        private void InitMessenger()
        {
            Messenger.Default.Register<StartDrivingMessage>(this, msg =>
            {
                if (this.driveWindow == null)
                {
                    this.driveWindow = new DriveWindow();
                    this.driveWindow.Show();
                }
                else
                {
                    this.driveWindow.Focus();
                }

                if (this.driveWindow.DataContext is DriveViewModel vm)
                {
                    vm.Drive(msg.Parameters);
                }
            });

            Messenger.Default.Register<OpenSettingsMessage>(this, msg =>
            {
                new SettingsWindow().ShowDialog();
            });

            Messenger.Default.Register<OpenLineManagementMessage>(this, msg =>
            {
                new LineManagementWindow().ShowDialog();
                Messenger.Default.Send(new RefreshDataMessage());
            });

            Messenger.Default.Register<OpenStopManagementMessage>(this, msg =>
            {
                new StopManagementWindow().ShowDialog();
                Messenger.Default.Send(new RefreshDataMessage());
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.driveWindow?.Close();
        }
    }
}
