using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GAPI.core
{
    public class GDriveApi
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Drive API .NET Quickstart";

        public IList<Google.Apis.Drive.v3.Data.File> GetFiles(string folderName = "root", string mimeType = "")
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            FilesResource.ListRequest listRequest;
            IList<Google.Apis.Drive.v3.Data.File> files;
            var folderId = "root";

            if (folderName!="root")
            {
                listRequest = service.Files.List();
                listRequest.Q = $"name='{folderName}' and mimeType='application/vnd.google-apps.folder'";
                listRequest.PageSize = 1;
                listRequest.Fields = "files(id, name)";
                files = listRequest.Execute().Files;
                folderId = files[0].Id;
                //Console.WriteLine($"{files[0].Name} -> {files[0].Id}");
                
            }
                       

            // Define parameters of request.
            listRequest = service.Files.List();
            //listRequest.Q = "mimeType='image/jpeg'";
            listRequest.Q = $"'{folderId}' in parents";
            if (mimeType!="") listRequest.Q += $" and mimeType='{mimeType}'";

            //Console.WriteLine(listRequest.Q);

            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, parents, mimeType, webViewLink, webContentLink, exportLinks)";

            //var folderId = "1KuRFKAulaDQETlf_o_55lSgNgWJb7hyY";


            // List files.
            files = listRequest.Execute().Files;

            return files;

        }

        public IList<Google.Apis.Drive.v3.Data.File> GetImages(string folderName = "root")
            => GetFiles(folderName, "image/jpeg");
    }
}
