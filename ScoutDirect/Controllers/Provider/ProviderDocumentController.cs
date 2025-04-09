using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderDocumentController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ProviderDocumentController(IMediator mediator, IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpPut("Document")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutDodument([FromForm] RegisterProviderDocumentCommand command)
        {
            string wwwPath = Environment.ContentRootPath;
            command.BaseVirtualPath = wwwPath;
            command.ProviderId = rProviderId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Document")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetDodument([FromForm] RegisterProviderDocumentCommand command)
        {
            string wwwPath = Environment.ContentRootPath;
            command.BaseVirtualPath = wwwPath;
            command.ProviderId = rProviderId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}