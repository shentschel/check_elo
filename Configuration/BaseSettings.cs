using System.Configuration;
using System.Text.Json.Serialization;

namespace check_elo.Configuration
{
    public class BaseSettings : ConfigurationElement
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [JsonPropertyName("Username")] public string User { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Password { get; set; }
    }
}