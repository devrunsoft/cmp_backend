using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Billing;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class AdminClientBilingInformationController : BaseAdminClientApiController
    {
        public AdminClientBilingInformationController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("Information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetInformation()
        {

            var result = await _mediator.Send(new GetInformationCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }


        [HttpPost("Information")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PostInformation([FromBody] PostBillingInformationCommand command)
        {
            command.CompanyId = rCompanyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromBody] DeleteBilingInformationCommand command)
        {
            command.CompanyId = rCompanyId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

