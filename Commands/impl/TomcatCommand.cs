using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using check_elo.Utils;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class TomcatCommand : BaseCommand<TomcatParameters>
    {
        public TomcatCommand(IXClient client, Settings settings, TomcatParameters parameters) : base(client, settings,
            parameters)
        {
        }

        public override bool Run()
        {
            var host = Helper.IfNullOrWhitespaceUse(Parameters.HostName, Settings.Elo.Host);
            var port = Helper.IfNullOrWhitespaceUse(Parameters.Port, Settings.Elo.Port);

            var tomcatUser = Helper.IfNullOrWhitespaceUse(Parameters.TomcatUser, Settings.Tomcat.User);
            var tomcatPwd = Helper.IfNullOrWhitespaceUse(Parameters.TomcatPassword, Settings.Tomcat.Password);


            var tomcatStatusUrl = $"http://{host}:{port}/manager/status";

            try
            {
                var httpClientHandler = new HttpClientHandler
                {
                    Credentials = new NetworkCredential(tomcatUser, Helper.DecryptPassword(tomcatPwd))
                };

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    var content = httpClient.GetStringAsync(tomcatStatusUrl).Result;

                    var success = CheckMemoryUsage(content);
                    if (success) success = CheckHeapUsage(content);

                    if (!success)
                    {
                        CheckResult.Message = "Could not parse memory information from Tomcat.";
                        CheckResult.ExitCode = ExitCode.Critical;
                    }
                }
            }
            catch (Exception ex)
            {
                CheckResult.Message = ex.Message;
                CheckResult.ExitCode = ExitCode.Critical;

                Log.Error($"Unable to connect to tomcat server due to: {ex.Message}", ex);
            }

            CheckResult.Commit();

            return CheckResult.ExitCode == ExitCode.Ok;
        }

        private bool CheckMemoryUsage(string content)
        {
            var memoryUsageRegex = new Regex("</h1>.*?Free\\Wmemory:\\W(?<FREE>\\d+).*?Max\\Wmemory:\\W(?<MAX>\\d+)");
            var findMemoryUsage = memoryUsageRegex.Match(content);

            if (!findMemoryUsage.Success) return false;

            var freeMemory = Convert.ToDouble(findMemoryUsage.Groups["FREE"].Value);
            var maxMemory = Convert.ToDouble(findMemoryUsage.Groups["MAX"].Value);
            var usedInPercent = 100 - freeMemory / maxMemory * 100;

            if (usedInPercent > Parameters.MemoryWarningThreshold)
                CheckResult.ExitCode = usedInPercent > Parameters.MemoryCriticalThreshold
                    ? ExitCode.Critical
                    : ExitCode.Warning;

            CheckResult.Message = $"Tomcat memory usage: {usedInPercent:0.##}%";
            CheckResult.PerformanceData = $"memory={usedInPercent:0.##};";

            return true;
        }

        private bool CheckHeapUsage(string content)
        {
            var heapUsageRegex = new Regex("PS\\WEden\\WSpace.*?\\((?<HEAP>\\d+)");
            var findHeapUsage = heapUsageRegex.Match(content);

            if (!findHeapUsage.Success) return false;

            var heapInPercent = Convert.ToDouble(findHeapUsage.Groups["HEAP"].Value);
            if (heapInPercent <= Parameters.HeapWarningThreshold)
                CheckResult.ExitCode = ExitCode.Ok;
            else
                CheckResult.ExitCode = heapInPercent < Parameters.HeapCriticalThreshold
                    ? ExitCode.Warning
                    : ExitCode.Critical;

            CheckResult.Message += $"| Tomcat heap usage: {heapInPercent:0.##}%";
            CheckResult.PerformanceData += $" heap usage={heapInPercent:0.##}";

            return true;
        }
    }
}