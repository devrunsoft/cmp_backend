using System;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

    }
}

