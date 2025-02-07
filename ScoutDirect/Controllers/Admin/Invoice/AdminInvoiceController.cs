using System;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Invoice
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminInvoiceController : BaseAdminApiController
    {
        public AdminInvoiceController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

