using System.IdentityModel.Tokens.Jwt;
using CMPEmail;
using System.Security.Claims;
using System.Text;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;
using Microsoft.Extensions.Options;
using CMPNatural.Application;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers
{
    public class DriverUserController : BaseDriverApiController
    {
        protected readonly ExpiresModel _expiresModel;
        private readonly IConfiguration _configuration;
        public DriverUserController(IMediator mediator , IConfiguration _configuration, IOptions<ExpiresModel> _expiresModel) : base(mediator)
        {
            this._configuration = _configuration;
            this._expiresModel = _expiresModel.Value;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new DriverGetProfileCommand() { DriverId = rDriverId});
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] DriverLoginCommand command)
        {
            if (command.Password.IsNullOrEmpty())
            {
                return Ok(new CommandResponse<object>() { Success = false, Message = "Password is not correct" });
            }

            var result = await _mediator.Send(command);
            if (!result.IsSucces())
            {
                return Ok(result);
            }

            var data = (DriverResponse)result.Data;

            //if (data.TwoFactor)
            //{
            //    var code = await data.twoFactor(_mediator);

            //    if (code != null)
            //    {
            //        sendEmail("Login Code", $"Code <strong>{code}</strong>", data.Email, $"{appSetting.adminHost}/", "Login");
            //        return Ok(new Success<object>()
            //        {
            //            Data = new
            //            {
            //                token = "TwoFactor",
            //                expiration = DateTime.Now,
            //            }
            //        });
            //    }
            //}

            var token = generatetoken(data);

            return Ok(new Success<object>()
            {
                Data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    isFirstLogin = false,
                    registered = false,
                    accepted = false,
                }
            });
        }

        private JwtSecurityToken generatetoken(DriverResponse data)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(_expiresModel.Admin),
            claims: get_claims(data.PersonId, data.Email, data.Id, data.FullName),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private Claim[] get_claims(Guid PersonId, string Email, long DriverId, string fullname)
        {
                List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, DriverId.ToString()) ,
                //new Claim(ClaimTypes.Role, Role),
                new Claim("PersonId", PersonId.ToString()),
                new Claim("Email", Email) };

            claims.Add(new Claim("FullName", fullname));
            return claims.ToArray();
        }
    }
}

