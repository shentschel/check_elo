using System;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Utils;
using pcm.IXClient7;

namespace check_elo.Commands.impl {
    public class PasswordCommand : BaseCommand<PasswordParameters> {
        public PasswordCommand(IXClient client, Settings settings, PasswordParameters parameters) : base(client, settings, parameters) {}
        
        public override bool Run() {
            string password = Parameters.UseDecrypt ? Helper.DecryptPassword(Parameters.Password) : Helper.EncryptPassword(Parameters.Password);
            
            Console.WriteLine(password);
            
            return true;
        }
    }
}