using CommandLine;

namespace check_elo.Parameters
{
    [Verb("doccount", HelpText = "Checks the amount of uploaded documents.")]
    public class DocCountParameters : SessionParameters
    {
        public static DocCountParameters CreateInstance() {
            return new DocCountParameters();
        }
    }
}