using CommandLine;

namespace check_elo.Parameters {
    [Verb("login", HelpText = "Executing an ELO login attempt.")]
    public class EloLoginParameters : LoginParameters {
        
    }
}