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
using ScoutDirect.Api.Controllers._Base;
using CMPNatural.Application.Commands.Company;
using Microsoft.AspNetCore.Authorization;
using CMPNatural.Application.Model;
using CMPNatural.Application.Responses;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterCompanyController : BaseApiController
    {
        protected readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public RegisterCompanyController(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            _mediator = mediator;
            _configuration = configuration;
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
                claims: get_claims(data.Type.ToString(), data.BusinessEmail, data.Id.ToString()),
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromBody] UpdateCompanyInput request)
        {

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
                //Password = request.Password

            });
            return Ok(result);

        }

     }
}


