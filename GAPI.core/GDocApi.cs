using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GAPI.core
{
    public class GDocApi
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/docs.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { DocsService.Scope.DocumentsReadonly };
        static string ApplicationName = "Google Docs API .NET Quickstart";

        protected DocsService service;

        public GDocApi()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                //var pscopes = new string[1];
                //pscopes[0]="https://www.googleapis.com/auth/documents";


                 credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "pizy123@gmail.com",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Docs API service.
            var service = new DocsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            this.service=service;
        }


        public Document GetDoc(string fileId)
        {   
            DocumentsResource.GetRequest request = this.service.Documents.Get(fileId);
            Document doc = request.Execute();
            return doc;
        }

        public void InsertImage(string docId, string imagePath)
        {
            var doc = GetDoc(docId);
            var docElementCount = doc.InlineObjects.Count;
            //var docContentCount = doc.PositionedObjects?.Count ?? 0;
            //Console.WriteLine($"{docElementCount} / {docContentCount}");

            List<Request> requests = new List<Request>();
                        
            var image = new InsertInlineImageRequest();
            
            image.Uri=imagePath;
            image.Location = new Location() { Index = docElementCount+1 };
            //image.ObjectSize = new Size() { Width = new Dimension() { Magnitude = 10, Unit = "PT" } };



            var request = new Request() { InsertInlineImage = image }; //, UpdateParagraphStyle = { ParagraphStyle = { Alignment="CENTER" } } };

            requests.Add(request);
                
            BatchUpdateDocumentRequest body = new BatchUpdateDocumentRequest();
            body.Requests=requests;

            BatchUpdateDocumentResponse response = this.service.Documents.BatchUpdate(body, docId).Execute();
        }


        public void CreateDoc(string fileName)
        {
            Document doc = new Document() { Title = "dokument testowy",  };
           
            doc = this.service.Documents.Create(doc).Execute();
            
        }

    }

}
