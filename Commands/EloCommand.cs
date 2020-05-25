using System;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using check_elo.Utils;
using pcm.IXClient7;

namespace check_elo.Commands
{
    public abstract class EloCommand<T> : BaseCommand<T> where T : LoginParameters
    {
        protected EloCommand(IXClient client, Settings settings, T parameters, CheckResult checkResult) : base(client, settings, parameters, checkResult)
        {
        }

        protected bool ExecuteEloCommand(Action callback)
        {
            try
            {
                var defaultIndexServerUrl =
                    Helper.GenerateDefaultIndexServerUrl(Settings.Elo.Host, Settings.Elo.Port,
                        Settings.Elo.ArchiveName);
                var indexServerUrl = Helper.IfNullOrWhitespaceUse(Parameters.IndexServerUrl, defaultIndexServerUrl);

                var user = Helper.IfNullOrWhitespaceUse(Parameters.EloUser, Settings.Elo.User);
                var password = Helper.IfNullOrWhitespaceUse(Parameters.EloPassword, Settings.Elo.Password);

                Client.Login(indexServerUrl, user, Helper.DecryptPassword(password), null, "check_elo");

                callback();
            }
            catch (Exception ex)
            {
                CheckResult.Message = ex.Message;
                CheckResult.ExitCode = ExitCode.Critical;

                Log.Error($"Cannot run WorkflowNodesCommand due to: {ex.Message}", ex);
            }
            finally
            {
                try
                {
                    Client.LogOut();
                }
                catch (Exception ex)
                {
                    Log.Warn("Unable to logout from index server", ex);
                }

                CheckResult.Commit();
            }

            return CheckResult.ExitCode == ExitCode.Ok;
        }
    }
}