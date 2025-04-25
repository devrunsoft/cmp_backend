using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.AppInformation
{
    [Authorize(Roles = "SuperAdmin")]
    public class AppInformationController : BaseAdminApiController
    {
        private readonly IWebHostEnvironment Environment;
        public AppInformationController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminAppInformationGetCommand());
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromForm] AdminAppInformationPutCommand request)
        {
            string wwwPath = Environment.ContentRootPath;
            request.BaseVirtualPath = wwwPath;

            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}

