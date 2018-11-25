using Newtonsoft.Json;

using System.IO;
using System.Text;

namespace BusSimulator.Core.Configuration
{
    public static class JsonApplicationConfiguration
    {
        private const string FILE_EXTENSION = ".json";

        public static T Load<T>(string fileNameWithoutExtension)
            where T : new()
        {
            var configPath = GetConfigPath(fileNameWithoutExtension);

            if (!File.Exists(configPath))
                return CreateDefaultConfigurationFile<T>(fileNameWithoutExtension);

            string content = File.ReadAllText(configPath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static void Save<T>(string fileNameWithoutExtension, T configuration)
            where T : new()
        {
            string configPath = GetConfigPath(fileNameWithoutExtension);
            string content = JsonConvert.SerializeObject(configuration);

            WriteToFile(content, configPath);
        }

        private static T CreateDefaultConfigurationFile<T>(string fileNameWithoutExtension)
            where T : new()
        {
            var config = new T();
            var configData = JsonConvert.SerializeObject(config);
            var configPath = GetConfigPath(fileNameWithoutExtension);

            WriteToFile(configData, configPath);

            return config;
        }

        private static void WriteToFile(string configData, string configPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            File.WriteAllText(configPath, configData, Encoding.UTF8);
        }

        private static string GetConfigPath(string fileNameWithoutExtension)
        {
            return Path.GetFullPath(fileNameWithoutExtension + FILE_EXTENSION);
        }
    }
}
