using System;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using check_elo.Utils;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class LoginCommand : BaseCommand<EloLoginParameters>
    {
        public LoginCommand(IXClient client, Settings settings, EloLoginParameters parameters, CheckResult checkResult) : base(client, settings,
            parameters, checkResult)
        {
        }

        public override bool Run()
        {
            Log.Debug($"Running command '{GetType()}'");
            
            CheckResult.Message = "ELO Login successful.";
            CheckResult.ExitCode = ExitCode.Ok;

            var generatedUrl =
                Helper.GenerateDefaultIndexServerUrl(Settings.Elo.Host, Settings.Elo.Port, Settings.Elo.ArchiveName);
            var url = Helper.IfNullOrWhitespaceUse(Parameters.IndexServerUrl, generatedUrl);
            var user = Helper.IfNullOrWhitespaceUse(Parameters.EloUser, Settings.Elo.User);
            var password = Helper.IfNullOrWhitespaceUse(Parameters.EloPassword, Settings.Elo.Password);

            try
            {
                Client.Login(url, user, Helper.DecryptPassword(password), null, "check_elo");
                Client.LogOut();
            }
            catch (Exception ex)
            {
                CheckResult.Message = ex.Message;
                CheckResult.ExitCode = ExitCode.Critical;

                Log.Error($"Unable to perform ELO login due to: {ex.Message}", ex);
            }

            CheckResult.Commit();
            return CheckResult.ExitCode == ExitCode.Ok;
        }
    }
}