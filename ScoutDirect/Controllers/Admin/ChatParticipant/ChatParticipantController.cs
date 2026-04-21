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
    [MenuAuthorize(MenuEnum.ParticipantConversation)]
    public class AdminChatParticipantController : BaseAdminApiController
    {
        public AdminChatParticipantController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Send/{ParticipantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("multipart/form-data")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long ParticipantId,  [FromForm] AdminSendCommonMessageCommand command)
        {
            //command.ChatCommonSessionId = ChatCommonSessionId;
            command.AdminId = AdminId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages/{ChatCommonSessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long ChatCommonSessionId, [FromQuery] AdminGetPaginateCommonMessageCommand command)
        {
            command.ChatCommonSessionId = ChatCommonSessionId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("ClientSessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] AdminGetPaginateCommonSessionsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost("Seen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> seen([FromBody] AdminSeenCommonMessageCommand command)
        {
            command.AdminId = AdminId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("CreateOrGet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CreateOrGet([FromBody] CreateChatCommonSessionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
