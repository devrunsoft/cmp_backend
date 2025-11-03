using System.Text.RegularExpressions;
using CMPFile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Identity.Client.Extensions.Msal;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers
{
    public class FileController : BaseApiController
    {
        private readonly IWebHostEnvironment Environment;
        private readonly IFileStorage fileStorage;
        public FileController(IMediator mediator, IWebHostEnvironment _environment, IFileStorage fileStorage) : base(mediator)
        {
            Environment = _environment;
            this.fileStorage = fileStorage;
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

        [AllowAnonymous]
        [HttpGet("proxy-file-minio/{**filePath}")]
        public async Task<IActionResult> ProxyFileMinio([FromRoute] string filePath, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return BadRequest("filePath is required.");

            filePath = Uri.UnescapeDataString(filePath);

            //// Try download from storage
            if (!await fileStorage.ExistsAsync(filePath, ct))
                return NotFound();

            var stream = await fileStorage.DownloadAsync(filePath, ct);

            var provider = new FileExtensionContentTypeProvider();
            var fileName = Path.GetFileName(filePath);
            if (!provider.TryGetContentType(fileName, out var contentType))
                contentType = "application/octet-stream";

            // Stream directly, enable range processing for media files
            return File(stream, contentType, fileDownloadName: fileName, enableRangeProcessing: true);
        }

    [HttpPost("proxy-file-minio")]
    [RequestSizeLimit(100_000_000)] // 100 MB example
    public async Task<IActionResult> UploadDFile([FromForm] IFormFile file, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file.");

        var url = await fileStorage.AppfileHandler(file);

        return Ok(new Success<string>() { Data = url });
    }
  }
}

