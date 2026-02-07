using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using CmpNatural.CrmManagment.Webhook;
using CMPNatural.Application;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api
{
    public class ProviderAuthController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        private readonly IConfiguration _configuration;
        private readonly AppSetting _appSetting;
        public ProviderAuthController(IMediator mediator, IConfiguration configuration, IWebHostEnvironment _environment, AppSetting appSetting) : base(mediator)
        {
            _configuration = configuration;
            Environment = _environment;
            _appSetting = appSetting;
        }


        [HttpPut("OperationalAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutOperational([FromBody] RegisterProviderOperationalAddressCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutOperationalAddress([FromBody] RegisterProviderOperationalAddressCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] RegisterProviderCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSucces())
                return Ok(result);

            var data = result.Data;
            if (result.IsSucces())
            {
                EmailSender(data);
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Login([FromBody] LoginProviderCommand command)
        {
            var result = await _mediator.Send(command);

            CommandResponse<DriverResponse>? resultDriver=null;
            if (!result.IsSucces())
            {
                 resultDriver = await _mediator.Send(new DriverLoginCommand() { Email = command.Email, Password = command.Password });

                if (!result.IsSucces() && !resultDriver.IsSucces())
                {
                    if (result.Data != null)
                    {
                        EmailSender(result.Data);
                    }
                    return Ok(result);
                }

            }


            bool isDriver = (resultDriver != null);
            var email = isDriver ? resultDriver.Data.Email : result.Data.Email;
            var Id = isDriver ? resultDriver.Data.ProviderId : result.Data.Id;
            long? DriverId = isDriver ? resultDriver.Data.Id : null;
            var isFirstLogin = isDriver ? true : result.Data.HasLogin;
            var IsDefault = isDriver ? resultDriver!.Data.IsDefault : true;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(email, Id, IsDefault, resultDriver != null, DriverId),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new Success<object>()
            {
                Data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    isFirstLogin = isFirstLogin
                }
            });
        }

        private void EmailSender(Provider data)
        {
            string host;

            if (Environment.IsDevelopment())
            {
                host = "http://localhost:16105";
            }
            else
            {
                host = _appSetting.host;
            }

            var link = host + "/api/Provider/Provider/Activate?activationLink=" + data.ActivationLink!.Value.ToString();
            sendEmail("Activation Link","\n\nThank you for signing up!\n\nTo activate your account and get started, please click the button below. This helps us verify your email address and complete your registration.\n\nIf you did not request this, you can safely ignore this message.\n",
                data.Email, link, "Activate Account\n");
        }

        private Claim[] get_claims( string Email, long ProviderId, bool IsDefault, bool IsDriver, long? DriverId)
        {
            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.NameIdentifier, ProviderId.ToString()) ,
                new Claim("Email", Email),
                new Claim("IsDefault", IsDefault==null? null: IsDefault.ToString()),
                new Claim("IsDriver", IsDriver.ToString()),
                new Claim("DriverId", DriverId==null ? "-1" : DriverId.ToString()),
            };
            return claims.ToArray();
        }

    }
}

