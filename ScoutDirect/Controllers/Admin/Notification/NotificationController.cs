using System;
using System.Threading.Tasks;
using CMPNatural.Api.Controllers.Admin;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Application.Commands.Admin.Notification;
using CMPNatural.Application.Model;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers
{
    public class NotificationController : BaseAdminApiController
    {
        public NotificationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllNotificationCommand command)
        {

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("UnSeen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UnSeen()
        {

            var result = await _mediator.Send(new AdminGetUnSeenNotificationCommand());
            return Ok(result);
        }
    }
}

