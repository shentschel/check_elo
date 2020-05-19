using CommandLine;

namespace check_elo.Parameters
{
    [Verb("elo-ocr", HelpText = "Checks ELO OCR.")]
    public class OcrParameters : SessionParameters
    {
        public static OcrParameters CreateInstance() {
            return new OcrParameters();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('p', "FullTextPath", HelpText = "File Path to the OCR Folder.")]
        public string FullTextPath { get; set; }
    }
}