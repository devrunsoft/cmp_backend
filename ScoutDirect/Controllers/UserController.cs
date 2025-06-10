using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Barbara.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CMPNatural.Application.Handlers.QueryHandlers;
using ScoutDirect.Application.Queries;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Commands.Company;
using ScoutDirect.Application.Responses;
using CmpNatural.CrmManagment.Webhook;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using CMPNatural.Application.Model;
using CMPEmail;
using Microsoft.Extensions.Options;
using CMPNatural.Core.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScoutDirect.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        protected readonly ExpiresModel _expiresModel;
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly UpdateContactTokenApi _updateContact;
        private readonly HighLevelSettings _highLevelSetting;
        private readonly AppSetting _appSetting;

        public UserController(IMediator mediator, IConfiguration configuration, IWebHostEnvironment env, UpdateContactTokenApi updateContact, IOptions<ExpiresModel> _expiresModel, HighLevelSettings _highLevelSetting, AppSetting appSetting)
        {
            _mediator = mediator;
            _configuration = configuration;
            _env = env;
            _updateContact = updateContact;
            this._expiresModel = _expiresModel.Value;
            this._highLevelSetting = _highLevelSetting;
            _appSetting = appSetting;
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Login([FromBody] LoginCompanyCommand command)
        {

            var result = await _mediator.Send(command);
            if (result.Success)
            {
                var company = (CompanyResponse) result.Data;

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(_expiresModel.Client),
                    claims: get_claims(company.Type.ToString(), company.BusinessEmail, company.Id.ToString(), company.Registered,company.ProfilePicture , company.FullName, company.PersonId),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

               var tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);

                _updateContact.send(command.BusinessEmail, tokenvalue);

                //return Ok(new
                //{
                //    token = tokenvalue,
                //    expiration = token.ValidTo,
                //    registered = company.Registered,
                //    accepted = company.Accepted,
                //});
                return Ok(new Success<object>()
                {
                    Data = new
                    {
                        token = tokenvalue,
                        expiration = token.ValidTo,
                        registered = company.Registered,
                        accepted = company.Accepted,
                    }
                });

            }

            return Ok(result);

        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ForgotPassword([FromBody] GetCompanyByEmailCommand command)
        {

            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                if(!(result!.Data is CompanyResponse))
                {
                    return Ok(new NoAcess() { });
                }

                var resultResend = await _mediator.Send(new CheckLinkCompanyCommand()
                {
                    CompanyId = ((CompanyResponse)result!.Data)!.Id.Value,
                    forgotPasswordLink = Guid.NewGuid()
                });

                if (resultResend.IsSucces())
                {
                    emailSender((CompanyResponse)resultResend.Data);
                }

            }

            return Ok(result);
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckResetPassword([FromQuery] CheckResetPasswordInput input)
        {
            var result = await _mediator.Send(new GetCompanyByEmailCommand()
            {
                Email = input.email,
            })!;

            if (!result.IsSucces())
            {
                return Ok(new NoAcess() { });
            }
            var company = ((CompanyResponse)result.Data);
            if (!company.Registered)
            {
                return Ok(new NoAcess() { });
            }

            if (company.ActivationLink!= input.forgotPasswordLink)
            {
                return Ok(new NoAcess() { });
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(_expiresModel.Client),
                claims: get_claims(company.Type.ToString(), company.BusinessEmail, company.Id.ToString(), company.Registered, company.ProfilePicture, company.FullName, company.PersonId),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );



            var html = System.IO.File.ReadAllText(@"./View/ForgotPassword.html");
            var tokenBearer = new JwtSecurityTokenHandler().WriteToken(token);
            html = html.Replace("{{Token}}", tokenBearer);
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }




        void emailSender(CompanyResponse data)
        {
            string host;

            if (_env.IsDevelopment())
            {
                host = "https://localhost:44202";
            }
            else
            {
                host = _appSetting.host;
            }

            var link = host + "/api/User/CheckResetPassword?forgotPasswordLink=" + data.ActivationLink!.Value.ToString()+ "&&email=" + data.BusinessEmail.ToString();

            new ForgotPassword(_highLevelSetting).send(data.BusinessEmail, link);
        }


        private Claim[] get_claims(string adminStatus, string businessEmail, string companyId,bool registered, string? ProfilePicture, string fullname, Guid PersonId)
        {
            List<Claim> claims = new List<Claim>() { new Claim("businessEmail", businessEmail), new Claim("CompanyId", companyId) };

            claims.Add(new Claim("Registered", registered.ToString()));
            claims.Add(new Claim("Type", adminStatus));
            claims.Add(new Claim("ProfilePicture", ProfilePicture??""));
            claims.Add(new Claim("FullName", fullname));
            claims.Add(new Claim("PersonId", PersonId.ToString()));

            return claims.ToArray();
        }



        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> SuccessForgotPassword()
        {
            var html = System.IO.File.ReadAllText(@"./View/SuccessForgotPassword.html");
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> FailureForgotPassword()
        {
            var html = System.IO.File.ReadAllText(@"./View/FailureForgotPassword.html");
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }
    }
}

