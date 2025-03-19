using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IWebHostEnvironment Environment;
        public FileController(IMediator mediator, IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        // GET: /<controller>/
        [HttpGet("proxy-file")]
        public async Task<IActionResult> ProxyFile(string filePath)
        {
            var path = $"{Environment.ContentRootPath}{filePath}";

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            string fileName = Path.GetFileName(filePath);
            var provider = new FileExtensionContentTypeProvider();
            string fileType;
            if (!provider.TryGetContentType(fileName, out fileType))
            {
                fileType = "application/octet-stream";  
            }
            if (fileBytes.Length == 0)
            {
            return Ok(new {});
            }

            return File(fileBytes, fileType, fileName);

        }
    }
}

