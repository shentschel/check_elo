using System.IO;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using check_elo.Utils;
using EloixClient.IndexServer;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class FileUploadCommand : EloCommand<FileUploadParameters>
    {
        public FileUploadCommand(IXClient client, Settings settings, FileUploadParameters parameters) : base(client,
            settings, parameters)
        {
        }

        public override bool Run()
        {
            return ExecuteEloCommand(CheckFileUpload);
        }

        private void CheckFileUpload()
        {
            var fileName = Helper.IfNullOrWhitespaceUse(Parameters.DocumentName, "check_elo.txt");
            var archivePath = Helper.IfNullOrWhitespaceUse(Parameters.ArchivePath, Settings.Elo.ArchiveFilePath);
            if (Parameters.IsCreateDocument)
            {
                File.WriteAllText(fileName,
                    $"This is a test document created by check_elo from host: {Settings.Elo.Host}");
            }
            else
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException($"Cannot upload file '{fileName}' to ELO. File not found. " +
                                                    "Please use parameter for creating a test document or provide one.");
            }

            Client.CreatePath(archivePath);
            var uploadSuccessful = Client.CreateAndUploadDoc("Check_ELO test document", fileName, null, 0,
                IXClient.StorageMode.Force);
            if (uploadSuccessful)
            {
                var deleteOptions = new DeleteOptions
                {
                    deleteFinally = false
                };
                var documentObjId = Client.LastEditInfo.document.objId;
                Client.IX.deleteSord(null, documentObjId, LockC.NO, deleteOptions);
                deleteOptions.deleteFinally = true;
                Client.IX.deleteSord(null, documentObjId, LockC.NO, deleteOptions);

                CheckResult.Message = $"Upload and archiving of document '{fileName}' was successful.";
                CheckResult.ExitCode = ExitCode.Ok;
            }
            else
            {
                CheckResult.Message = "Unable to upload file.";
                CheckResult.ExitCode = ExitCode.Critical;
            }
        }
    }
}