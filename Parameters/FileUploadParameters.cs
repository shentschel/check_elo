using CommandLine;

namespace check_elo.Parameters
{
    [Verb("filedoc", HelpText = "Tests the file upload by uploading a file and deleting it afterwards.")]
    public class FileUploadParameters : LoginParameters
    {
        public static FileUploadParameters CreateInstance() {
            return new FileUploadParameters();
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('p', "path",
            HelpText = "Archive path for uploading the document. (e.g.: \"\\Administration\\Test\"")]
        public string ArchivePath { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('d', "document",
            HelpText =
                "Name of the document for uploading. (Please place the file in the same directory as this executable.")]
        public string DocumentName { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Option('c', "create", Default = false, HelpText = "Create a test document instead of uploading one?")]
        public bool IsCreateDocument { get; set; }
    }
}