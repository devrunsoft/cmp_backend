using System;
using CMPNatural.Application;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderChatMessageController : BaseProviderApiController
    {
        public ProviderChatMessageController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("multipart/form-data")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromForm] ParticipantSendCommonMessageCommand command)
        {
            command.ParticipantId = rIsDriver? rDriverId.Value: rProviderId;
            command.ParticipantType = rIsDriver? ParticipantType.Driver : ParticipantType.Provider;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ParticipantGetPaginateCommonMessageCommand command)
        {
            command.ParticipantId = rIsDriver ? rDriverId.Value : rProviderId; ;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Seen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> seen([FromBody] ParticipantSeenCommonMessageCommand command)
        {
            command.ParticipantId = rIsDriver ? rDriverId.Value : rProviderId; ;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
