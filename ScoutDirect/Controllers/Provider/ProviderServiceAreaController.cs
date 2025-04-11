using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.ProviderServiceArea;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderServiceAreaController : BaseProviderApiController
    {
        public ProviderServiceAreaController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminGetAllProviderServiceAreaCommand()
            {
                ProviderId = rProviderId,
            });
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ServiceAreaInput request)
        {
            var result = await _mediator.Send(new AdminAddProviderServiceAreaCommand(request, rProviderId));
            return Ok(result);
        }

        [HttpPost("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PostAll([FromBody] List<ServiceAreaInput> request)
        {
            var result = await _mediator.Send(new AdminAddAllProviderServiceAreaCommand(request, rProviderId));
            return Ok(result);
        }


        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminDeleteProviderServiceAreaCommand(Id, rProviderId));
            return Ok(result);
        }
    }
}

