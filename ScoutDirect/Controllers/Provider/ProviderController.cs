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


namespace CMPNatural.Api.Controllers.Admin.Provider
{

    [Route("api/provider/provider")]
    public class AppProviderController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public AppProviderController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminGetProviderCommand() { Id = rProviderId });
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPutProviderCommand(input, rProviderId , wwwPath));

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
                return "<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n    <meta charset=\"UTF-8\">\n  " +
                    "  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Activation Success</title>\n   " +
                    " <style>\n        body {\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n     " +
                    "       padding: 0;\n        }\n        .container {\n            width: 100%;\n            max-width: 600px;\n            margin: 50px auto;\n         " +
                    "   background-color: #ffffff;\n            padding: 20px;\n            border-radius: 10px;\n          " +
                    "  box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);\n        }\n        h1 {\n            color: #4c8e3b;\n        }\n        p {\n        " +
                    "    font-size: 16px;\n            color: #333333;\n        }\n        .button {\n            display: inline-block;\n            padding: 10px 20px;\n            margin-top: 20px;\n            background-color: #4c8e3b;\n            color: white;\n            text-decoration: none;\n            border-radius: 5px;\n        }\n        .button:hover {\n            background-color: #3b6e29;\n        }\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Activation Successful!</h1>\n        <p>Thank you for activating your account. Your account is now fully active, and you can start using our services.</p>\n        <p>If you have any questions, feel free to contact our support team.</p>\n        <a href=\"https://provider.app-cmp.com/logout\" class=\"button\">Go to Login</a>\n    </div>\n</body>\n</html>\n";
            }
            else
            {
                return "<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n    <meta charset=\"UTF-8\">\n  " +
                    "  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n   " +
                    " <title>Activation Failure</title>\n    <style>\n        body {\n            font-family: Arial, sans-serif;\n          " +
                    "  background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n        }\n        .container {\n         " +
                    "   width: 100%;\n            max-width: 600px;\n            margin: 50px auto;\n            background-color: #ffffff;\n          " +
                    "  padding: 20px;\n            border-radius: 10px;\n            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);\n        }\n        h1 {\n         " +
                    "   color: #e74c3c;\n        }\n        p {\n            font-size: 16px;\n            color: #333333;\n        }\n        .button {\n       " +
                    "     display: inline-block;\n            padding: 10px 20px;\n            margin-top: 20px;\n            background-color: #e74c3c;\n     " +
                    "       color: white;\n            text-decoration: none;\n            border-radius: 5px;\n        }\n        .button:hover {\n            background-color: #c0392b;\n        }\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Activation Failed</h1>\n        <p>Unfortunately, we were unable to activate your account due to an error. Please try again or contact our support team for assistance.</p>\n        <p>We apologize for the inconvenience.</p>\n        <a href=\"https://provider.app-cmp.com/logout\" class=\"button\">Back To Application</a>\n    </div>\n</body>\n</html>\n";
            }

        }
    }
}

