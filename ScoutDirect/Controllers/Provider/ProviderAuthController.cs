using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.Menu;
using CMPNatural.Application.Commands.Provider.Biling;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers
{
    public class ProviderAuthController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        private readonly IConfiguration _configuration;
        public ProviderAuthController(IMediator mediator, IConfiguration configuration, IWebHostEnvironment _environment) : base(mediator)
        {
            _configuration = configuration;
            Environment = _environment;
        }

        [HttpPut("Document")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutDodument([FromForm] RegisterProviderDocumentCommand command)
        {
            string wwwPath = Environment.ContentRootPath;
            command.BaseVirtualPath = wwwPath;
            command.ProviderId = rProviderId;

            var result = await _mediator.Send(command);
            return Ok(result);
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

        [HttpPut("Billing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> PutBilling([FromBody] ProviderAddBilingCommand command)
        {
            //command.ProviderId = rProviderId;
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
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(data.Email, data.Id),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new Success<object>()
            {
                Data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                }
            });
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Login([FromBody] LoginProviderCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSucces())
                return Ok(result);

            var data = result.Data;
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(data.Email, data.Id),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new Success<object>()
            {
                Data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                }
            });
        }

        private Claim[] get_claims( string Email, long ProviderId)
        {
            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.NameIdentifier, ProviderId.ToString()) ,
                new Claim("Email", Email)
            };
            return claims.ToArray();
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

    }
}

