using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class DocCountCommand : EloCommand<DocCountParameters>
    {
        public DocCountCommand(IXClient client, Settings settings, DocCountParameters parameters) : base(client,
            settings, parameters)
        {
        }

        public override bool Run()
        {
            return ExecuteEloCommand(CountDocuments);
        }

        private void CountDocuments()
        {
            var docCount = Client.IX.getArchiveStatistics(1L).maxDocId;

            CheckResult.Message = string.Format("Documents in archive: {0} | maxDocId={0}", docCount);
            CheckResult.ExitCode = ExitCode.Ok;

            if (Parameters.StatsOnly) return;

            if (docCount > Parameters.WarningThreshold)
                CheckResult.ExitCode = docCount > Parameters.CriticalThreshold
                    ? ExitCode.Critical
                    : ExitCode.Warning;
        }
    }
}