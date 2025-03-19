using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Company;
using CMPPayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using ScoutDirect.Api.Controllers._Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Payment
{
    public class PaymentController : BaseClientApiController
    {
        public PaymentController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        [AllowAnonymous]
        public async Task<ActionResult> Pay()
        {
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        [AllowAnonymous]
        public async Task<ActionResult> PayButton()
        {
            //new PaymentConfiguration().CreatePayment();

            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        [AllowAnonymous]
        public async Task<ActionResult> CheckStatus([FromQuery] string session_id)
        {

            return Ok();
        }
    }
}

