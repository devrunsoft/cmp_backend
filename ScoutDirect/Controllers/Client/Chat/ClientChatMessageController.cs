using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Client;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Client.Chat
{
    public class ClientChatMessageController : BaseClientApiController
    {
        public ClientChatMessageController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Send/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long OperationalAddressId, [FromBody] ClientSendMessageCommand command)
        {
            command.ClientId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long OperationalAddressId, [FromQuery] ClientGetPaginateMessageCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Seen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> seen([FromBody] ClientSeenMessageCommand command)
        {
            command.ClientId = rCompanyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

