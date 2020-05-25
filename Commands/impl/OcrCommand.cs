using System;
using System.IO;
using System.Linq;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using check_elo.Utils;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class OcrCommand : BaseCommand<OcrParameters>
    {
        public OcrCommand(IXClient client, Settings settings, OcrParameters parameters, CheckResult checkResult) : base(client, settings,
            parameters, checkResult)
        {
        }

        public override bool Run()
        {
            try
            {
                var path = Helper.IfNullOrWhitespaceUse(Parameters.FullTextPath, Settings.Elo.FullTextPath);
                var files = new DirectoryInfo(path).GetFiles("*.*", SearchOption.AllDirectories);
                var expiredFilesCount = files
                    .Select(file => DateTime.UtcNow - file.CreationTimeUtc)
                    .Count(timeSinceCreation => timeSinceCreation > TimeSpan.FromDays(1));

                CheckResult.Message = $"{expiredFilesCount} files in OCR Folder";
                CheckResult.PerformanceData = $"file-count={expiredFilesCount}";
                CheckResult.ExitCode = ExitCode.Ok;

                if (!Parameters.StatsOnly)
                    if (expiredFilesCount >= Parameters.WarningThreshold)
                        CheckResult.ExitCode = expiredFilesCount >= Parameters.CriticalThreshold
                            ? ExitCode.Critical
                            : ExitCode.Warning;
            }
            catch (Exception ex)
            {
                CheckResult.Message = ex.Message;
                CheckResult.ExitCode = ExitCode.Critical;

                Log.Error($"Unable to check OCR state due to: {ex.Message}", ex);
            }

            CheckResult.Commit();

            return CheckResult.ExitCode == ExitCode.Ok;
        }
    }
}