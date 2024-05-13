using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace AdminPanal.Helpers
{
    public class PictuerSettings
    {
        public static string UploadImage(IFormFile file, string FolderName)
        {
            //1-get Folder Path
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FolderName);

            // 2 - set filename Formate

            var filename = Guid.NewGuid() + file.FileName;

            var filePath = Path.Combine(FolderPath, filename);

            var fs = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fs);

            return Path.Combine("images\\products", filename);
        }

        public static void DeleteFile(string FolderName, string filename)
        {

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FolderName, filename);
            if(File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
