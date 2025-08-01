using System;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Message
{
    public class AdminChatMessageController : BaseAdminApiController
    {
        public AdminChatMessageController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Send/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long ClientId, [FromBody] AdminSendMessageCommand command)
        {
            command.ClientId = ClientId;
            command.AdminId = AdminId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages/{ClientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long ClientId, [FromQuery] AdminGetPaginateMessageCommand command)
        {
            command.CompanyId = ClientId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminGetPaginateSessionsCommand());
            return Ok(result);
        }
    }
}

