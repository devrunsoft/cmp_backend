using CMPNatural.Application.Commands.Admin.Company;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Public
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/public/company-import")]
    public class CompanyImportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyImportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ImportExcel([FromForm] AdminImportCompanyExcelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
