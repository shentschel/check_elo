using CommandLine;

namespace check_elo.Parameters
{
    [Verb("tomcat", HelpText = "Checks tomcat heap and memory usage")]
    public class TomcatParameters : BaseParameters
    {
        public static TomcatParameters CreateInstance() {
            return new TomcatParameters();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("mwarn", Default = 85, HelpText = "% warning threshold for tomcat memory.")]
        public double MemoryWarningThreshold { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("mcrit", Default = 95, HelpText = "% critical threshold for tomcat memory.")]
        public double MemoryCriticalThreshold { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("hwarn", Default = 85, HelpText = "% warning threshold for tomcat heap memory.")]
        public double HeapWarningThreshold { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("hcrit", Default = 95, HelpText = "% critical threshold for tomcat heap memory.")]
        public double HeapCriticalThreshold { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('h', "hostname", HelpText = "Hostname of the server running tomcat.")]
        public string HostName { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('p', "port", HelpText = "Tomcat server port.")]
        public string Port { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('u', "user", HelpText = "Username for tomcat login.")]
        public string TomcatUser { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("password", HelpText = "Password for tomcat login.")]
        public string TomcatPassword { get; set; }
    }
}