using System;
using System.Diagnostics;
using check_elo.Commands.impl;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using CommandLine;
using log4net;
using pcm.IXClient7;

namespace check_elo
{
    internal class CheckElo
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CheckElo));

        private static void Main(string[] args)
        {
            var configFilePath = Process.GetCurrentProcess().ProcessName + ".json";
            var settings = ConfigReader.ReadConfigFile(configFilePath);

            if (settings == null)
            {
                var errorMsg =
                    $"Unable to find settings file. Please provide a settings file with name: {configFilePath}";
                Log.Error(errorMsg);

                new CheckResult {Message = errorMsg, ExitCode = ExitCode.Critical}.Commit();

                Environment.Exit(2);
            }

            var client = IXClient.Instance;

            Parser.Default.ParseArguments<
                    DocCountParameters,
                    FileUploadParameters,
                    IxSessionParameters,
                    EloLoginParameters,
                    SqlConnectionParameters,
                    TomcatParameters,
                    WebappParameters,
                    WorkflowNodesParameters,
                    OcrParameters, 
                    PasswordParameters>(args)
                .MapResult((DocCountParameters parameters) => new DocCountCommand(client, settings, parameters).Run(),
                    (FileUploadParameters parameters) => new FileUploadCommand(client, settings, parameters).Run(),
                    (IxSessionParameters parameters) => new IxSessionCommand(client, settings, parameters).Run(),
                    (EloLoginParameters parameters) => new LoginCommand(client, settings, parameters).Run(),
                    (SqlConnectionParameters parameters) =>
                        new SqlConnectionCommand(client, settings, parameters).Run(),
                    (TomcatParameters parameters) => new TomcatCommand(client, settings, parameters).Run(),
                    (WebappParameters parameters) => new WebappCommand(client, settings, parameters).Run(),
                    (WorkflowNodesParameters parameters) =>
                        new WorkflowNodesCommand(client, settings, parameters).Run(),
                    (OcrParameters parameters) => new OcrCommand(client, settings, parameters).Run(),
                    (PasswordParameters parameters) => new PasswordCommand(client, settings, parameters).Run(), 
                    parseErrors =>
                    {
                        var response = new CheckResult
                        {
                            Message = "Failed to parse parameters.",
                            ExitCode = ExitCode.Unknown
                        };
                        response.Commit();

                        foreach (var error in parseErrors) Log.Error($"Unable to parse Parameter '{error.Tag}'.");

                        return false;
                    }
                );
        }
    }
}