using check_elo.Utils;
using CommandLine;

namespace check_elo.Parameters {
    
    [Verb("crypt", Hidden = true, HelpText = "encrypts or decrypts a password")]
    public class PasswordParameters {
        public static PasswordParameters CreateInstance() {
            return new PasswordParameters();
        }
        

        [Option('d', "decrypt", Default = false, HelpText = "decrypts a cipher")]
        public bool UseDecrypt {
            get;
            set;
        }

        [Option('p', "password", Required = true, HelpText = "Password string")]
        public string Password {
            get;
            set;
        }
    }
}