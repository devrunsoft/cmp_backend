using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.ProviderServiceArea;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    public class ServiceAreaController : BaseAdminProviderApiController
    {
        public ServiceAreaController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
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

