using BusSimulator.Ui.Logic.Interfaces;

using Microsoft.Win32;

using System.Windows;

namespace BusSimulator.Ui.Dialogs
{
    public class FileDialog : IDialogWindow
    {
        public (bool, string) ShowFileDialog(object owner, string filter)
        {
            var fd = new OpenFileDialog();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                fd.Filter = filter;
            }
            return (fd.ShowDialog(owner as Window).Value, fd.FileName);
        }
    }
}
