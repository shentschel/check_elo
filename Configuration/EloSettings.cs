namespace check_elo.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EloSettings : DatabaseSettings
    {

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string ArchiveName { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string ArchiveFilePath { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string FullTextPath { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Port { get; set; }
    }
}