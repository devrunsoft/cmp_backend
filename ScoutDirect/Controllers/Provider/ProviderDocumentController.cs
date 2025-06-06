using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;


namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderDocumentController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ProviderDocumentController(IMediator mediator, IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [RequestSizeLimit(100_000_000)]
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
        public async Task<ActionResult> GetDodument([FromForm] ProviderGetDocumentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}