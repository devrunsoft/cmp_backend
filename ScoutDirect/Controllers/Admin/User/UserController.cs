using System;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.User
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class UserController : BaseAdminApiController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _mediator.Send(new AdminGetAllInvoiceCommand()
            {
            });
            return Ok(result);
        }

        [HttpGet("MenuAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Access()
        {
            var result = await _mediator.Send(new AdminGetAllMenuByAdminCommand()
            {
                AdminId = AdminId
            });
            return Ok(result);
        }

    }
}

