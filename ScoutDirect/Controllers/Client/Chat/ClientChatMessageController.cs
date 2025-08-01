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

        [HttpPost("Send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ClientSendMessageCommand command)
        {
            command.ClientId = rCompanyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ClientGetPaginateMessageCommand command)
        {
            command.CompanyId = rCompanyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

