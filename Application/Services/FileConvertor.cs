using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbara.Application.Services
{
    public class FileConvertor
    {
        public void Convert2Mp3(string assemblyDirectory, string inputAudioPath, string outputAudioPath)
        {
            //string mp3Path = AudioPath.Replace(FileName + "." + exName, FileName + ".mp3");

            //var AssemblyDirectory = HttpContext.Request.MapPath("~/Tools/ffmpeg/bin/");
            //string executablePath = Path.Combine(assemblyDirectory, "Tools/ffmpeg/bin/ffmpeg.exe");

            var info = new ProcessStartInfo();
            info.FileName = assemblyDirectory;// string.Format("\"{0}\"", assemblyDirectory);
            //  info.WorkingDirectory = IOUtils.AssemblyDirectory;
            //128k
            info.Arguments = string.Format("-y -i \"{0}\" -codec:a libmp3lame -b:a 32k \"{1}\"", inputAudioPath, outputAudioPath);

            info.RedirectStandardInput = false;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            using (var proc = new Process())
            {
                proc.StartInfo = info;
                proc.Start();
            }
        }

    }
}
