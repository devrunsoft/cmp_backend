using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace CMPNatural.Application.Services
{
	public class FileHandler
	{

    public  static string? fileHandler(string BaseVirtualPath, IFormFile file, string key)
        {
            if (file == null)
                return null;

            var id = key;
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{id}{extension}";
            SaveFile(BaseVirtualPath, file, fileName);
            return $"/FileContent/{BaseVirtualPath.Split('/').Last()}/{fileName}";
        }

        public static void SaveFile(string BaseVirtualPath, IFormFile file, string fileName)
        {
            var filePath = Path.Combine(BaseVirtualPath, fileName);
            if (!Directory.Exists(BaseVirtualPath))
            {
                Directory.CreateDirectory(BaseVirtualPath);
            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Position = 0;
                file.CopyTo(fileStream);
            }
        }

    }
}

