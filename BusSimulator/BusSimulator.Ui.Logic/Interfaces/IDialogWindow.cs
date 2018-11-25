namespace BusSimulator.Ui.Logic.Interfaces
{
    public interface IDialogWindow
    {
        (bool, string) ShowFileDialog(object owner, string filter);
    }
}
