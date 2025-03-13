using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.AdminManagment;
using CMPNatural.Application.Commands.Admin.Menu;
using CMPNatural.Application.Commands.Admin.provider;
using CMPNatural.Application.Handlers.Admin.AdminManagment;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminManagmentController : BaseAdminApiController
    {
        protected readonly IMediator _mediator;
        public AdminManagmentController(IMediator mediator): base(mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] AdminInput input)
        {
            var result = await _mediator.Send(new AdminAddAdminCommand()
            {
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                IsActive = input.IsActive,
                Password = input.Password
            });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromBody] AdminInput input)
        {
            var result = await _mediator.Send(new AdminUpdateAdminCommand()
            {
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                IsActive = input.IsActive,
                Password = input.Password,
                Id = Id
            });
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminDeleteAdminCommand()
            {
                Id = Id
            });
            return Ok(result);
        }

        [HttpPut("MenuAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutMenu([FromBody] AdminAddMenuCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("MenuAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetMenu()
        {
            var result = await _mediator.Send(new AdminGetallMenuCommand());
            return Ok(result);
        }

        [HttpGet("MenuAccess/Admin/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetMenuByAdmin([FromRoute] long Id)
        {
            var result = await _mediator.Send(new AdminGetAllMenuByAdminCommand() {  AdminId = Id});
            return Ok(result);
        }
    }
}

