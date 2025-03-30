using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Entities;
using ScoutDirect.Api.Controllers._Base;
using Hangfire.MemoryStorage.Database;

namespace CMPNatural.Api.Controllers.Admin.Auth
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminAuthController : BaseApiController
    {
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public AdminAuthController(IMediator mediator, IConfiguration configuration):base(mediator)
		{
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("Code")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] AdminLoginCodeCommand command)
        {

            var result = await _mediator.Send(command);
            if (!result.IsSucces())
            {
                return Ok(result);
            }

            var data = (AdminEntity)result.Data;
            var token = generatetoken(data);

            return Ok(new Success<object>()
            {
                Data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                }
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] AdminLoginCommand command)
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

            var data = (AdminEntity)result.Data;

            if (data.TwoFactor)
            {
              var code = await data.twoFactor(_mediator);

                if (code != null)
                {
                    sendEmail("Login Code",$"Code <strong>{code}</strong>", data.Email);
                    return Ok(new Success<object>()
                    {
                        Data = new
                        {
                            token = "TwoFactor",
                            expiration = DateTime.Now,
                        }
                    });
                }

            }

            var token = generatetoken(data);

            return Ok(new Success<object>() {
            Data = new
                     {
                         token = new JwtSecurityTokenHandler().WriteToken(token),
                         expiration = token.ValidTo,
                     }
                 });
        }

        private JwtSecurityToken generatetoken(AdminEntity data)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(30),
            claims: get_claims(data.PersonId, data.Email, data.Role, data.Id),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private Claim[] get_claims(Guid PersonId, string Email,string Role , long AdminId)
        {
            List<Claim> claims = new List<Claim>() { new Claim("isAdmin", "true"),
                new Claim(ClaimTypes.NameIdentifier, AdminId.ToString()) ,
                new Claim(ClaimTypes.Role, Role), new Claim("PersonId", PersonId.ToString()), new Claim("Email", Email) };
            return claims.ToArray();
        }
    }
}

