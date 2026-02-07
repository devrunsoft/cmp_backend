using System;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;

namespace CMPNatural.Api.Controllers.Admin.Message
{
    [MenuAuthorize(MenuEnum.Conversation)]
    public class AdminChatMessageController : BaseAdminApiController
    {
        public AdminChatMessageController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Send/{ClientId}/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("multipart/form-data")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long ClientId, [FromRoute] long OperationalAddressId, [FromForm] AdminSendMessageCommand command)
        {
            command.OperationalAddressId = OperationalAddressId;
            command.ClientId = ClientId;
            command.AdminId = AdminId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages/{ClientId}/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long OperationalAddressId, [FromRoute] long ClientId, [FromQuery] AdminGetPaginateMessageCommand command)
        {
            command.OperationalAddressId = OperationalAddressId;
            command.ClientId = ClientId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("ClientSessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetPaginateClientSessionsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("ClientSessions/Sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetClientSessions([FromQuery] ClientInput input)
        {
            var result = await _mediator.Send(new AdminGetPaginateSessionsCommand() { ClientId = input.ClientId });
            return Ok(result);
        }

        [HttpGet("ClientSessions/Sessions/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetClientSessions([FromRoute] long OperationalAddressId)
        {
            var result = await _mediator.Send(new AdminGetChatSessionCommand() { OperationalAddressId = OperationalAddressId });
            return Ok(result);
        }

        [HttpPost("Seen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> seen([FromBody] AdminSeenMessageCommand command)
        {
            command.AdminId = AdminId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
