using System;
using CMPNatural.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Barbara.Application.Responses;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Core.Entities;
using ScoutDirect.Core.Models;
using CMPNatural.Core.Entities;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterCompanyController : ControllerBase
    {
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public RegisterCompanyController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] RegisterCompanyCommand command)
        {
            if (command.Password != command.RePassword)
            {
                return Ok(new CommandResponse<object>() { Success = false, Message = "Password is not correct" });
            }

            var result = await _mediator.Send(command);

            if (result.Id == null)
            {
                return Ok(new CommandResponse<object>() { Success = false, Message = "This Company is exist" });
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(result.Type.ToString(), result.BusinessEmail, result.Id.ToString()),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                //  register_type = result.RegisterType,
                accepted = false,
            });
        }

        private Claim[] get_claims(string adminStatus, string businessEmail, string companyId)
        {
            List<Claim> claims = new List<Claim>() { new Claim("businessEmail", businessEmail), new Claim("CompanyId", companyId) };

            claims.Add(new Claim("Registered", "false"));
            claims.Add(new Claim("Type", adminStatus));

            return claims.ToArray();
        }
    }
}


