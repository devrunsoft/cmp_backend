using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Admin.Company;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Company
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class CompanyController : BaseAdminApiController
    {
        public CompanyController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllCompanyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminGetCompanyCommand() { Id = Id });
            return Ok(result);
        }

        [HttpPut("ChangeStatus/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ChangeStatus([FromRoute] long Id, [FromBody] AdminChangeStatusClientCommand command)
        {
            command.ClientId = Id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

