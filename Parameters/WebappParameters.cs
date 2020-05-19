using CommandLine;

namespace check_elo.Parameters
{
    [Verb("webapp", HelpText = "Checks the functionality of a specified webapp.")]
    public class WebappParameters : BaseParameters
    {
        public static WebappParameters CreateInstance() {
            return new WebappParameters();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('n', "name", Default = "ix", Required = true,
            HelpText = "Name of the webapp to check (i.e.: am, ix, dm, as, ...).")]
        public string Name { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('i', "url", HelpText = "Complete url to the webapp.")]
        public string Url { get; set; }
    }
}