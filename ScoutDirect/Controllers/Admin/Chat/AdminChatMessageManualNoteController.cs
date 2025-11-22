using System;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers.Admin.Message
{
    [MenuAuthorize(MenuEnum.Conversation)]
    public class AdminChatMessageManualNoteController : BaseAdminApiController
    {
        public AdminChatMessageManualNoteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Send/{ClientId}/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long ClientId, [FromRoute] long OperationalAddressId, [FromBody] AdminSendMessageManualNoteCommand command)
        {
            command.OperationalAddressId = OperationalAddressId;
            command.ClientId = ClientId;
            command.AdminId = AdminId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long OperationalAddressId, [FromQuery] AdminGetPaginateMessageManualNoteCommand command)
        {
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

