using CommandLine;

namespace check_elo.Parameters
{
    [Verb("wfnodes",
        HelpText = "Search a workflow node for one or more users and checks how long the node state did not change.")]
    public class WorkflowNodesParameters : LoginParameters
    {
        public static WorkflowNodesParameters CreateInstance() {
            return new WorkflowNodesParameters();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('w', "warn", Default = 10,
            HelpText = "Max duration in minutes for a node to stay before emitting a warning state.")]
        public int WarningMinutes { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('c', "crit", Default = 20,
            HelpText = "Max duration in minutes for a node to stay before emitting a critical state.")]
        public int CriticalMinutes { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option("wfusers", Required = true, HelpText = "Usernames for which the workflow nodes shall be fetched.")]
        public string WorkflowUsers { get; set; }
    }
}