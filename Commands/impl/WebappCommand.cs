using System;
using System.Diagnostics;
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
    public class WebappCommand : BaseCommand<WebappParameters>
    {
        public WebappCommand(IXClient client, Settings settings, WebappParameters parameters) : base(client, settings,
            parameters)
        {
            Log.Debug("Created Webapp Command");
        }

        public override bool Run()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var stopwatch = new Stopwatch();

                    var webappUrl = Helper.IfNullOrWhitespaceUse(Parameters.Url, CreateWebappUrlFromName());

                    stopwatch.Start();
                    httpClient.Timeout = TimeSpan.FromMilliseconds(10000);
                    var responseMessage = httpClient.GetAsync(webappUrl).Result;
                    stopwatch.Stop();

                    CheckResult.PerformanceData = $"elapsed time={stopwatch.ElapsedMilliseconds} ms";

                    if (responseMessage != null)
                    {
                        var requestWasSuccessful = responseMessage.StatusCode == HttpStatusCode.OK;
                        if (!requestWasSuccessful)
                        {
                            CheckResult.Message =
                                $"Connecting to webapp '{Parameters.Name}' failed. HTTP status code: {responseMessage.StatusCode.ToString()}";
                            CheckResult.ExitCode = ExitCode.Critical;
                        }
                        else
                        {
                            var content = responseMessage.Content.ReadAsStringAsync().Result;
                            CheckWebappContent(content);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CheckResult.Message = ex.Message;
                    CheckResult.ExitCode = ExitCode.Critical;

                    Log.Error($"Unable to perform WebappCommand due to: {ex.Message}", ex);
                }

                CheckResult.Commit();
            }

            return CheckResult.ExitCode == ExitCode.Ok;
        }

        private string CreateWebappUrlFromName()
        {
            string url;
            var hostPart = $"http://{Settings.Elo.Host}:{Settings.Elo.Port}";
            var commandPart = "?__cmd__=status&mode=html";
            var urlPart = $"{Parameters.Name}-{Settings.Elo.ArchiveName}";

            switch (Parameters.Name)
            {
                case "ix":
                    commandPart = "?cmd=status";
                    url = $"{hostPart}/{urlPart}/{Parameters.Name}{commandPart}";
                    break;
                case "sx":
                    url = $"{hostPart}/{urlPart}/if{commandPart}";
                    break;
                case "am":
                    url = $"{hostPart}/{Parameters.Name}-eloam/am{commandPart}";
                    break;
                case "web":
                case "ig2":
                    url = $"{hostPart}/{urlPart}/status{commandPart}";
                    break;
                default:
                    url = $"{hostPart}/{urlPart}/{Parameters.Name}{commandPart}";
                    break;
            }

            return url;
        }

        private void CheckWebappContent(string content)
        {
            if (content.ToLower().Contains("<div class=\"statusheadrunning\">"))
            {
                CheckResult.Message = $"Webapp '{Parameters.Name}' up and running.";
                CheckResult.ExitCode = ExitCode.Ok;
            }
            else
            {
                const string regexGroupName = "MESSAGE";
                var regexString = string.Format("<tr class=({0})>{1}</td>{1}<td>(?<{2}>{1})(</td>|</p>)",
                    "\"StatusTableError\"|\"StatusTableWarn\"|\"StatusTableFatal\"",
                    "(.*?)",
                    regexGroupName);
                var match = new Regex(regexString).Match(content);

                if (!match.Success) return;

                var containsWarningsMessage = !content.ToLower().Contains("warnings - should be fixed");
                CheckResult.Message = match.Groups[regexGroupName].Value;
                CheckResult.ExitCode = containsWarningsMessage ? ExitCode.Warning : ExitCode.Critical;
            }
        }
    }
}