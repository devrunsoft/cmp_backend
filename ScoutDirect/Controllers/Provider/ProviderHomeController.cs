using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Provider;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderHomeController : BaseProviderApiController
    {
        public ProviderHomeController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetReport()
        {
            var result = await _mediator.Send(new ProviderGetReportCommand() { ProviderId = rProviderId });
            return Ok(result);
        }
    }
}

