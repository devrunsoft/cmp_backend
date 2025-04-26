using System;
using CMPNatural.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Api.Controllers._Base;
using CMPNatural.Application.Commands.Company;
using Microsoft.AspNetCore.Authorization;
using CMPNatural.Application.Model;
using CMPNatural.Application.Responses;
using System.Net;
using CmpNatural.CrmManagment.Webhook;
using CMPNatural.Application.Commands.Billing;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterCompanyController : BaseClientApiController
    {
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env ;

        public RegisterCompanyController(IMediator mediator, IConfiguration configuration, IWebHostEnvironment env) : base(mediator)
        {
            _mediator = mediator;
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Activate([FromQuery] Guid activationLink)
        {
            var resultCompany = await _mediator.Send(new ActivateCompanyCompany()
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
                    "    font-size: 16px;\n            color: #333333;\n        }\n        .button {\n            display: inline-block;\n            padding: 10px 20px;\n            margin-top: 20px;\n            background-color: #4c8e3b;\n            color: white;\n            text-decoration: none;\n            border-radius: 5px;\n        }\n        .button:hover {\n            background-color: #3b6e29;\n        }\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Activation Successful!</h1>\n        <p>Thank you for activating your account. Your account is now fully active, and you can start using our services.</p>\n        <p>If you have any questions, feel free to contact our support team.</p>\n        <a href=\"https://client.app-cmp.com/logout\" class=\"button\">Go to Login</a>\n    </div>\n</body>\n</html>\n";
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
                    "       color: white;\n            text-decoration: none;\n            border-radius: 5px;\n        }\n        .button:hover {\n            background-color: #c0392b;\n        }\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Activation Failed</h1>\n        <p>Unfortunately, we were unable to activate your account due to an error. Please try again or contact our support team for assistance.</p>\n        <p>We apologize for the inconvenience.</p>\n        <a href=\"https://client.app-cmp.com/logout\" class=\"button\">Back To Application</a>\n    </div>\n</body>\n</html>\n";
            }

        }

        [HttpPut]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ResendEmail()
        {
            var result = await _mediator.Send(new ResendEmailCompanyCommand()
            {
                CompanyId = rCompanyId,
                ActivationLink= Guid.NewGuid()
            });
            if (result.IsSucces())
            {
                EmailSender((CompanyResponse) result.Data);
            }
            return Ok(result);
        }


        void EmailSender(CompanyResponse data)
        {
            string host;

            if (_env.IsDevelopment())
            {
                host = "https://localhost:7089";
            }
            else
            {
                host = "https://api.app-cmp.com";
            }

            var link = host + "/api/RegisterCompany/Activate?activationLink=" + data.ActivationLink!.Value.ToString();
            data.ActivationLinkGo = link;
            new ActivationLink().send(data);
        }


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] RegisterCompanyCommand command)
        {
            if (command.Password != command.RePassword)
            {
                return Ok(new CommandResponse<object>() { Success = false, Message = "Password is not correct" });
            }

            var result = await _mediator.Send(command);
            if (!result.IsSucces())
            {
                return Ok(result);
            }

            var data =(CompanyResponse) result.Data;
            if (data.Id == null)
            {
                return Ok(new CommandResponse<object>() { Success = false, Message = "This Company is exist" });
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(data.Type.ToString(), data.BusinessEmail, data.Id.ToString(), data.ProfilePicture),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            EmailSender(data);


            return Ok(new Success<object>()
            {
                Data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    registered = false,
                    accepted = false,
                }
            });
        }

        private Claim[] get_claims(string adminStatus, string businessEmail, string companyId, string? ProfilePicture)
        {
            List<Claim> claims = new List<Claim>() { new Claim("businessEmail", businessEmail), new Claim("CompanyId", companyId) };

            claims.Add(new Claim("Registered", "false"));
            claims.Add(new Claim("Type", adminStatus));
            claims.Add(new Claim("ProfilePicture", ProfilePicture ?? ""));

            return claims.ToArray();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            });
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPut("UploadProfilePicture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UploadProfilePicture([FromForm] ProfilePictureInput input)
        {
            string wwwPath = _env.ContentRootPath;
            var result = await _mediator.Send(new UploadProfilePictureCommand()
            {
                CompanyId = rCompanyId,
                ProfilePicture = input.ProfilePicture,
                Path = wwwPath
            });

            return Ok(result);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromBody] UpdateCompanyInput request)
        {
            var resultBillingAddress = await _mediator.Send(new AddOrUpdateBillingAddressCommand()
            {
                CompanyId = rCompanyId,
                Address = request.BillingAddress,
                City = request.City,
                ZIPCode = request.ZIPCode,
                State = request.State
            });

            var result = await _mediator.Send(new UpdateCompanyCommand()
            {
                CompanyId = rCompanyId,
                CompanyName = request.CompanyName,
                Position = request.Position,
                PrimaryFirstName = request.PrimaryFirstName,
                PrimaryLastName = request.PrimaryLastName,
                PrimaryPhonNumber = request.PrimaryPhonNumber,
                SecondaryFirstName = request.SecondaryFirstName,
                SecondaryLastName = request.SecondaryLastName,
                SecondaryPhoneNumber = request.SecondaryPhoneNumber,
            });
            return Ok(result);
        }




        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordInput input)
        {

            if(input.Password!= input.RePassword)
            {
                return Ok(new NoAcess() { });
            }


            var result = await _mediator.Send(new ResetPasswordCompanyCommand()
            {
                CompanyId = rCompanyId,
                Password = input.Password,
                RePassword = input.RePassword,
            })!;

            return Ok(result);

        }
    }
}


