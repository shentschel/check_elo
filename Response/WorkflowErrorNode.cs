namespace check_elo.Response
{
    public class WorkflowErrorNode
    {
        public WorkflowErrorNode(string name, string eloUser, int minutesSinceActivation)
        {
            Name = name;
            EloUser = eloUser;
            MinutesSinceActivation = minutesSinceActivation;
        }

        public string Name { get; }

        public string EloUser { get; }

        public int MinutesSinceActivation { get; }

        public ExitCode ExitCode { get; set; }
    }
}