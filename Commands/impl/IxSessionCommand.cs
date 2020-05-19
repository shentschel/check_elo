using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using EloixClient.IndexServer;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class IxSessionCommand : EloCommand<IxSessionParameters>
    {
        public IxSessionCommand(IXClient client, Settings settings, IxSessionParameters parameters) : base(client,
            settings, parameters)
        {
        }

        public override bool Run()
        {
            return ExecuteEloCommand(CountSessions);
        }

        private void CountSessions()
        {
            var sessionCount = Client.IX.getSessionInfos(new SessionInfoParams()).Length;

            CheckResult.Message = string.Format("Active IX sessions: {0} | ixsessions={0}", sessionCount);
            CheckResult.ExitCode = ExitCode.Ok;

            if (Parameters.StatsOnly) return;

            if (sessionCount > Parameters.WarningThreshold)
                CheckResult.ExitCode = sessionCount > Parameters.CriticalThreshold
                    ? ExitCode.Critical
                    : ExitCode.Warning;
        }
    }
}