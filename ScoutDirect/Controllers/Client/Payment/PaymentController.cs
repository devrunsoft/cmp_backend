using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Client.Payment;
using CMPNatural.Application.Commands.Company;
using CMPPayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Payment
{
    public class PaymentController : BaseApiController
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
           var result = await _mediator.Send(new ClientInvoicePaidCommand() { CheckoutSessionId = session_id });

            var html = result.Data
                ? GenerateHtmlMessage("Payment Successful", "Thank you! Your payment has been successfully processed.", "Back to Application", "https://client.app-cmp.com/", "#28a745")
                : GenerateHtmlMessage("Payment Failed", "Sorry, the payment could not be verified. Please contact support.", "Back to Application", "https://client.app-cmp.com/", "#dc3545");

            return Content(html, "text/html");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        [AllowAnonymous]
        public async Task<ActionResult> Cancel([FromQuery] string session_id)
        {
            var html = GenerateHtmlMessage(
                "Payment Cancelled",
                "Your payment process was cancelled. If this was a mistake, please try again.",
                "Back to Application",
                "https://client.app-cmp.com/",
                "#d9534f"
            );

            await _mediator.Send(new ClientInvoicePaidCommand() { CheckoutSessionId = session_id });

            return Content(html, "text/html");
        }

        private string GenerateHtmlMessage(string title, string message, string buttonText, string buttonUrl, string color)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <title>{title}</title>
        <style>
            body {{ font-family: Arial, sans-serif; text-align: center; padding: 50px; }}
            h1 {{ color: {color}; }}
            .btn {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                color: #fff;
                background-color: #007bff;
                border: none;
                border-radius: 5px;
                text-decoration: none;
                margin-top: 20px;
                cursor: pointer;
            }}
            .btn:hover {{
                background-color: #0056b3;
            }}
        </style>
    </head>
    <body>
        <h1>{title}</h1>
        <p>{message}</p>
        <a href='{buttonUrl}' class='btn'>{buttonText}</a>
    </body>
    </html>";
        }


    }
}

