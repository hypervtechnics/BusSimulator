namespace BusSimulator.Core.Configuration
{
    public class JsonDataConfiguration
    {
        public JsonDataConfiguration()
        {
            this.JsonDataRepositoryFile = "Data";
        }

        public string JsonDataRepositoryFile { get; set; }
    }
}
