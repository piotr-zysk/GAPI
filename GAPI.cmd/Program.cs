using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAPI.core;

namespace GAPI.cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageFiles = new List<string>();
            var preffix="";
            var files = new GDriveApi().GetImages("__dev").OrderByDescending(f=>f.MimeType).ThenBy(f=>f.Name);
            foreach (var file in files)
            {
                if (file.MimeType == "application/vnd.google-apps.folder")
                    preffix = "[folder]";
                else
                    preffix = "[file]";
                Console.WriteLine($"{preffix}:{file.Name}->{file.Parents?[0]} ({file.MimeType})\r\n{file.WebViewLink}\r\n{file.WebContentLink}\r\n");
                imageFiles.Add(file.WebViewLink);
            }
            

            var gdocs = new GDocApi();

            var fileId = "1ZNBNn_R36A8Hn9XQTLmeEI-QkOXbxBUxkXa4bLQg3EE";

            var doc = gdocs.GetDoc(fileId);

            Console.WriteLine($"{doc.Title}");

            foreach(var image in imageFiles)
            {
                gdocs.InsertImage(fileId, image);
            }
            

            //gdocs.CreateDoc("test");
            



        }
    }
}

// https://developers.google.com/drive/api/v3/folder