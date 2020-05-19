using System;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using log4net;
using pcm.IXClient7;

namespace check_elo.Commands
{
    public abstract class BaseCommand<T> : ICommand {
        protected readonly CheckResult CheckResult = new CheckResult();
        protected readonly IXClient Client;
        protected readonly T Parameters;
        protected readonly Settings Settings;

        protected BaseCommand(IXClient client, Settings settings, T parameters)
        {
            Settings = settings;
            Parameters = parameters;
            Client = client;
            
            Log.Debug("Created BaseCommand");
        }

        protected ILog Log {
            get {
                return LogManager.GetLogger(GetType());
            }
        }

        public abstract bool Run();

        protected bool ExecuteCommand(Action callback) {
            try {
                callback();

            } catch (Exception ex) {
                CheckResult.Message = ex.Message;
                CheckResult.ExitCode = ExitCode.Critical;
                
                Log.Error($"Unable to execute command '{GetType()}' due to: {ex.Message}", ex);
            }
            
            CheckResult.Commit();

            return CheckResult.ExitCode == ExitCode.Ok;
        }
    }
}