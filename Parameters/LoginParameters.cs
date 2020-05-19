using CommandLine;

namespace check_elo.Parameters
{
    public abstract class LoginParameters : BaseParameters
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('i', "indexserver", HelpText = "URL of the index server.")]
        public string IndexServerUrl { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('u', "user", HelpText = "ELO user for login.")]
        public string EloUser { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("password", HelpText = "ELO user password for login")]
        public string EloPassword { get; set; }
    }
}