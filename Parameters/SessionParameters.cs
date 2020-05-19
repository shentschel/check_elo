using CommandLine;

namespace check_elo.Parameters {
    public class SessionParameters : LoginParameters {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('w', "warn", Default = 100, HelpText = "Threshold before emitting a warning state.")]
        public double WarningThreshold { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('c', "crit", Default = 200, HelpText = "Threshold before emitting a critical state.")]
        public double CriticalThreshold { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('s', "stats", HelpText = "Show stats only without emitting critical or warning states")]
        public bool StatsOnly { get; set; }
    }
}