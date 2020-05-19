using System.Configuration;

namespace check_elo.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Settings : ConfigurationSection
    {
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public EloSettings Elo { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public DatabaseSettings Database { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public TomcatSettings Tomcat { get; set; }
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public PasswordSettings Password {
            get;
            set;
        }
    }
}