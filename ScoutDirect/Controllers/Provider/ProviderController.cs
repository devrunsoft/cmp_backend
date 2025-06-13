using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Invoice;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;
using ScoutDirect.Api.Controllers._Base;
using CMPNatural.Application.Command;
using CMPNatural.Core.Models;

namespace CMPNatural.Api.Controllers.Admin.Provider
{

    [Route("api/provider/provider")]
    public class AppProviderController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        private readonly AppSetting appSetting;
        public AppProviderController(IMediator mediator,
            IWebHostEnvironment _environment, AppSetting appSetting) : base(mediator)
        {
            Environment = _environment;
            this.appSetting = appSetting;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminGetProviderCommand() { Id = rProviderId });
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPutProviderCommand(input, rProviderId , wwwPath));
            return Ok(result);
        }

        [HttpPut("UpdateServiceAndLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UpdateServiceAndLocation([FromBody] ProviderUpdateServiceAndLocationCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Activate([FromQuery] Guid activationLink)
        {
            var resultCompany = await _mediator.Send(new ProviderActivateEmailCommand()
            {
                activationLink = activationLink,
            })!;

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = viewCheck(resultCompany.IsSucces())
            };
        }

        private string viewCheck(bool isSuccess)
        {
            if (isSuccess)
            {
                return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Activation Success</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 50px auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }}
        h1 {{
            color: #4c8e3b;
        }}
        p {{
            font-size: 16px;
            color: #333333;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            background-color: #4c8e3b;
            color: white;
            text-decoration: none;
            border-radius: 5px;
        }}
        .button:hover {{
            background-color: #3b6e29;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Activation Successful!</h1>
        <p>Thank you for activating your account. Your account is now fully active, and you can start using our services.</p>
        <p>If you have any questions, feel free to contact our support team.</p>
        <a href=""{appSetting.providerHost}/logout"" class=""button"">Go to Login</a>
    </div>
</body>
</html>
";
            }
            else
            {
                return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Activation Failure</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 50px auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
        }}
        h1 {{
            color: #e74c3c;
        }}
        p {{
            font-size: 16px;
            color: #333333;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            margin-top: 20px;
            background-color: #e74c3c;
            color: white;
            text-decoration: none;
            border-radius: 5px;
        }}
        .button:hover {{
            background-color: #c0392b;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Activation Failed</h1>
        <p>Unfortunately, we were unable to activate your account due to an error. Please try again or contact our support team for assistance.</p>
        <p>We apologize for the inconvenience.</p>
        <a href=""{appSetting.providerHost}/logout"" class=""button"">Back To Application</a>
    </div>
</body>
</html>
";
            }


        }
    }
}

