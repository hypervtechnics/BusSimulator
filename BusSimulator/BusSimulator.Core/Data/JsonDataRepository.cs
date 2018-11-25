namespace BusSimulator.Core.Data
{
    public abstract class JsonDataRepository
    {
        protected string filename;

        protected JsonDataRepository(string filename)
        {
            this.filename = filename;
        }
    }
}
