using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace CMPNatural.Application.Services
{
	public class FileHandler
	{
        public static string ProviderDriverfileHandler(string BaseVirtualPath, IFormFile file, string key, dynamic uniqueId, string path)
        {
            return AppfileHandler(BaseVirtualPath, file, key, $"Provider/{uniqueId}/Driver/{path}");
        }

        public static string ProviderVehiclefileHandler(string BaseVirtualPath, IFormFile file, string key, dynamic uniqueId, string path)
        {
            return AppfileHandler(BaseVirtualPath, file, key,  $"Provider/{uniqueId}/Vehicle/{path}");
        }

        public static string ProviderfileHandler(string BaseVirtualPath, IFormFile file, string key, dynamic uniqueId, string path)
        {
            return AppfileHandler(BaseVirtualPath, file, key, $"Provider/{uniqueId}/{path}");
        }

        public static string AppfileHandler(string BaseVirtualPath, IFormFile file, string key, string folderName)
        {
            if (file == null)
                return null;

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{key}{extension}";

            string path= $"{BaseVirtualPath}/FileContent/{folderName}";

            SaveFile(path, file, fileName);
            return $"/FileContent/{folderName}/{fileName}";
        }

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

