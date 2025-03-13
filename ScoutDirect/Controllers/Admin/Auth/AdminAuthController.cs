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
using Microsoft.AspNetCore.Components.Forms;
using System.Configuration;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Entities;

namespace CMPNatural.Api.Controllers.Admin.Auth
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminAuthController : Controller
    {
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public AdminAuthController(IMediator mediator, IConfiguration configuration)
		{
            _mediator = mediator;
            _configuration = configuration;
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

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(30),
                claims: get_claims(data.PersonId , data.Email, data.Role, data.Id),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new Success<object>() {
            Data = new
                     {
                         token = new JwtSecurityTokenHandler().WriteToken(token),
                         expiration = token.ValidTo,
                     }
                 });
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

