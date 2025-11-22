
using CMPNatural.Application;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.TermsConditions
{
    [MenuAuthorize(MenuEnum.TermsConditions)]
    public class TermsConditionsController : BaseAdminApiController
    {
        public TermsConditionsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminTermsConditionsPaginateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] TermsConditionsInput request)
        {
            var result = await _mediator.Send(new AdminTermsConditionsAddCommand(request));
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] TermsConditionsInput request)
        {
            var result = await _mediator.Send(new AdminTermsConditionsEditCommand(request, Id));
            return Ok(result);
        }


        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminTermsConditionsDeleteCommand()
            {
                Id = Id
            });
            return Ok(result);
        }


    }
}

