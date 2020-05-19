using System.Text.Json.Serialization;

namespace check_elo.Configuration
{
    public class DatabaseSettings : BaseSettings
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [JsonPropertyName("HostName")] public string Host { get; set; }
    }
}