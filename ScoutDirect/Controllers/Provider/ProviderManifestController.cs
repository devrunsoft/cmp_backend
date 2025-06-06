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
    public class ProviderManifestController : BaseProviderApiController
    {
        public ProviderManifestController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ProviderGetAllManifestCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetId([FromRoute] long Id)
        {
            var result = await _mediator.Send(new ProviderGetManifestCommand() { Id = Id , ProviderId = rProviderId });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new ProviderGetAllActiveManifestCommand() { ProviderId = rProviderId });
            return Ok(result);
        }

        [HttpGet("Complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetComplete()
        {
            var result = await _mediator.Send(new ProviderGetAllCompleteManifestCommand() { ProviderId = rProviderId });
            return Ok(result);
        }
    }
}

