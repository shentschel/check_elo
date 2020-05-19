using CommandLine;

namespace check_elo.Parameters
{
    [Verb("sql", HelpText = "Checks the sql connection.")]
    public class SqlConnectionParameters : BaseParameters
    {
        public static SqlConnectionParameters CreateInstance() {
            return new SqlConnectionParameters();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('H', "hostname", HelpText = "Hostname of the database server.")]
        public string HostName { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('d', "database", HelpText = "Name of the database.")]
        public string DatabaseName { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('u', "user", HelpText = "User name for database login.")]
        public string DatabaseUserName { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("password", HelpText = "Password for database login.")]
        public string DatabasePassword { get; set; }
    }
}