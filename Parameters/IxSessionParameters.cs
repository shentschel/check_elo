using CommandLine;

namespace check_elo.Parameters
{
    [Verb("ixsessions", HelpText = "Checks count of active index server sessions")]
    public class IxSessionParameters : SessionParameters
    {
    }
}