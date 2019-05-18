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
            var preffix="";
            var files = new GDriveApi().GetImages("__dev").OrderByDescending(f=>f.MimeType).ThenBy(f=>f.Name);
            foreach (var file in files)
            {
                if (file.MimeType == "application/vnd.google-apps.folder")
                    preffix = "[folder]";
                else
                    preffix = "[file]";
                Console.WriteLine($"{preffix}:{file.Name}->{file.Parents?[0]} ({file.MimeType})");
            }


        }
    }
}

// https://developers.google.com/drive/api/v3/folder