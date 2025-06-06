using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin
{
    public class CommonController : BaseApiController
    {
        private readonly IWebHostEnvironment Environment;
        public CommonController(IMediator mediator, IWebHostEnvironment _environment) :base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet("Logo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Logo()
        {
            var result = await _mediator.Send(new LogoCommand());

            string wwwPath = Environment.ContentRootPath;
            var filePath = (wwwPath+ result.Data);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Logo file not found.");

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string mimeType))
            {
                mimeType = "application/octet-stream";  // Fallback MIME type
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, mimeType);
        }


        [HttpGet("Information")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Information()
        {
            var result = await _mediator.Send(new InformationCommand());
            return Ok(result);
        }

    }
}

