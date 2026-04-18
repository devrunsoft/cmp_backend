using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using CmpNatural.CrmManagment.Webhook;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Model;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;
using Microsoft.Extensions.Options;
using CMPEmail;

namespace CMPNatural.Api
{
    public class ProviderAuthController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        private readonly IConfiguration _configuration;
        private readonly AppSetting _appSetting;
        private readonly IProviderReposiotry _providerRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ExpiresModel _expiresModel;

        public ProviderAuthController(
            IMediator mediator,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            AppSetting appSetting,
            IProviderReposiotry providerRepository,
            IDriverRepository driverRepository,
            IOptions<ExpiresModel> expiresModel) : base(mediator)
        {
            _configuration = configuration;
            Environment = environment;
            _appSetting = appSetting;
            _providerRepository = providerRepository;
            _driverRepository = driverRepository;
            _expiresModel = expiresModel.Value;
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ForgotPassword([FromBody] CreateProviderForgotPasswordLinkCommand command)
        {
            var providerResult = await _mediator.Send(command);
            if (providerResult.IsSucces())
            {
                emailSender(providerResult.Data);
                return Ok(providerResult);
            }

            var driverResult = await _mediator.Send(new CreateDriverForgotPasswordLinkCommand()
            {
                Email = command.Email
            });

            if (driverResult.IsSucces())
            {
                emailSender(driverResult.Data);
                return Ok(new Success<object>());
            }

            return Ok(providerResult);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckResetPassword([FromQuery] CheckResetPasswordInput input)
        {
            var provider = (await _providerRepository.GetAsync(x => x.Email == input.email)).FirstOrDefault();
            if (provider != null && provider.ActivationLink == input.forgotPasswordLink)
            {
                return BuildResetPasswordHtml(
                    provider.Email,
                    provider.Id,
                    true,
                    false,
                    null,
                    provider.PersonId?.ToString() ?? Guid.Empty.ToString());
            }

            var driver = (await _driverRepository.GetAsync(x => x.Email == input.email, query => query.Include(x => x.ProviderDriver))).FirstOrDefault();
            if (driver != null && driver.ActivationLink == input.forgotPasswordLink)
            {
                var isDefault = driver.ProviderDriver?.Any(x => x.IsDefault) == true;
                return BuildResetPasswordHtml(
                    driver.Email,
                    0,
                    isDefault,
                    true,
                    driver.Id,
                    driver.PersonId.ToString());
            }

            return Ok(new NoAcess() { });
        }

        [HttpPut("ResetPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordInput input)
        {
            if (input.Password != input.RePassword)
            {
                return Ok(new NoAcess() { });
            }

            if (rIsDriver && rDriverId.HasValue)
            {
                var driver = (await _driverRepository.GetAsync(x => x.Id == rDriverId.Value)).FirstOrDefault();
                if (driver == null || driver.ActivationLink == null)
                {
                    return Ok(new NoAcess() { });
                }

                driver.Password = input.Password;
                driver.ActivationLink = null;
                await _driverRepository.UpdateAsync(driver);
                return Ok(new Success<object>());
            }

            var provider = (await _providerRepository.GetAsync(x => x.Id == rProviderId)).FirstOrDefault();
            if (provider == null || provider.ActivationLink == null)
            {
                return Ok(new NoAcess() { });
            }

            provider.Password = input.Password;
            provider.ActivationLink = null;
            await _providerRepository.UpdateAsync(provider);
            return Ok(new Success<object>());
        }

        private ActionResult BuildResetPasswordHtml(string email, long providerId, bool isDefault, bool isDriver, long? driverId, string personId)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(_expiresModel.Client),
                claims: get_claims(email, providerId, isDefault, isDriver, driverId, personId),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var html = System.IO.File.ReadAllText(@"./View/ForgotPasswordProvider.html");
            var tokenBearer = new JwtSecurityTokenHandler().WriteToken(token);
            html = html.Replace("{{Token}}", tokenBearer);
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        void emailSender(Provider data)
        {
            string host = _appSetting.host;
            var link = host + "/api/Provider/ProviderAuth/CheckResetPassword?forgotPasswordLink=" + data.ActivationLink!.Value + "&&email=" + data.Email;
            sendEmail("Reset Password", "", data.Email, link, "Reset");
        }

        void emailSender(Driver data)
        {
            string host = _appSetting.host;
            var link = host + "/api/Provider/ProviderAuth/CheckResetPassword?forgotPasswordLink=" + data.ActivationLink!.Value + "&&email=" + data.Email;
            sendEmail("Reset Password", "", data.Email, link, "Reset");
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

            CommandResponse<DriverResponse>? resultDriver = null;
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
            var IsDefault = isDriver ? resultDriver!.Data.IsDefault : true;
            var email = isDriver ? resultDriver.Data.Email : result.Data.Email;
            string personId = isDriver ? resultDriver.Data.PersonId.ToString() : result.Data.PersonId.ToString();
            var Id = isDriver ? 0 : result.Data.Id;
            long? DriverId = isDriver ? resultDriver.Data.Id : null;
            var isFirstLogin = isDriver ? true : result.Data.HasLogin;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(email, Id, IsDefault, resultDriver != null, DriverId, personId),
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
            sendEmail("Activation Link", "\n\nThank you for signing up!\n\nTo activate your account and get started, please click the button below. This helps us verify your email address and complete your registration.\n\nIf you did not request this, you can safely ignore this message.\n",
                data.Email, link, "Activate Account\n");
        }

        private Claim[] get_claims(string Email, long ProviderId, bool IsDefault, bool IsDriver, long? DriverId, string PersonId)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, ProviderId.ToString()) ,
                new Claim("Email", Email),
                new Claim("PersonId", PersonId),
                new Claim("IsDefault", IsDefault.ToString()),
                new Claim("IsDriver", IsDriver.ToString()),
                new Claim("DriverId", DriverId==null ? "-1" : DriverId.ToString()),
            };
            return claims.ToArray();
        }
    }
}
