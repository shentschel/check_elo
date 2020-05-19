using System;
using System.IO;
using System.Text.Json;
using check_elo.Response;
using log4net;

namespace check_elo.Configuration
{
    public class ConfigReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigReader));

        public static Settings ReadConfigFile(string configFilePath)
        {
            Settings settings = null;

            try
            {
                if (string.IsNullOrWhiteSpace(configFilePath))
                    throw new ArgumentException("configFilePath parameter cannot be null or empty.");

                var configJson = File.ReadAllText(configFilePath);
                settings = JsonSerializer.Deserialize<Settings>(configJson);
            }
            catch (Exception ex)
            {
                var response = new CheckResult
                {
                    Message = $"Unable to read config file due to: {ex.Message}.",
                    ExitCode = ExitCode.Critical
                };
                response.Commit();

                Log.Error($"Unable to read config file due to: {ex.Message}.", ex);
            }

            return settings;
        }
    }
}