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
    }
}

