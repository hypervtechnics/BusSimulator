namespace BusSimulator.Ui.Logic.Messages
{
    public class RefreshDataMessage
    {
        public RefreshDataMessage(bool lines = false)
        {
            this.Lines = lines;
        }

        public bool Lines { get; set; }
    }
}
