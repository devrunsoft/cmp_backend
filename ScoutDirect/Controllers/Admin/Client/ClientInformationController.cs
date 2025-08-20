using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Api.Controllers.Admin;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Admin.Company;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Application.Commands.Client.Representation;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Model;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using ScoutDirect.Application.Responses;


namespace CMPNatural.Api.Controllers
{
    public class ClientInformationController : BaseAdminApiController
    {
        private readonly IWebHostEnvironment _env;
        public ClientInformationController(IMediator mediator, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : base(mediator)
        {
            _env = env;
        }


        [HttpGet("{clientId}/Representation/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long clientId, [FromRoute] long OperationalAddressId)
        {
            var result = await _mediator.Send(new ClientMenuRepresentationCommand()
            {
                CompanyId = clientId,
                OperationalAddressId = OperationalAddressId
            });
            return Ok(result);
        }

        [HttpPut("{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long clientId, [FromBody] UpdateCompanyInput request)
        {
            var resultBillingAddress = await _mediator.Send(new AddBilingInformationCommand()
            {
                BilingInformationInputs = request.InformationInput.BilingInformationInputs,
                CorporateAddress = request.InformationInput.CorporateAddress,
                CompanyId = clientId
            });

            var result = await _mediator.Send(new UpdateCompanyCommand()
            {
                CompanyId = clientId,
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

        [RequestSizeLimit(100_000_000)]
        [HttpPut("UploadProfilePicture/{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UploadProfilePicture([FromRoute] long clientId, [FromForm] ProfilePictureInput input)
        {
            string wwwPath = _env.ContentRootPath;
            var result = await _mediator.Send(new UploadProfilePictureCommand()
            {
                CompanyId = clientId,
                ProfilePicture = input.ProfilePicture,
                Path = wwwPath
            });

            return Ok(result);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] AddCompanyInput request)
        {


            var result = await _mediator.Send(new AdminAddCompanyCommand()
            {
                CompanyName = request.CompanyName,
                Position = request.Position,
                PrimaryFirstName = request.PrimaryFirstName,
                PrimaryLastName = request.PrimaryLastName,
                PrimaryPhonNumber = request.PrimaryPhonNumber,
                SecondaryFirstName = request.SecondaryFirstName,
                SecondaryLastName = request.SecondaryLastName,
                SecondaryPhoneNumber = request.SecondaryPhoneNumber,
                BusinessEmail = request.BusinessEmail
            });
            if (result.IsSucces())
            {
                var data = (result.Data as Company);
                 var resultBillingAddress = await _mediator.Send(new AddBilingInformationCommand()
                 {
                     BilingInformationInputs = request.InformationInput.BilingInformationInputs,
                     CorporateAddress = request.InformationInput.CorporateAddress,
                     CompanyId = data.Id,
                 });
                if (result.IsSucces())
                {
                    SendToClient("Your Account Credentials", $"<p style=\"margin: 5px 0;\"> <strong>Username/Email:</strong> <span style=\"color: #16a085; font-family: monospace;\">{data.BusinessEmail}</span> </p> <p style=\"margin: 5px 0;\"> <strong>Password:</strong> <span style=\"color: #c0392b; font-family: monospace;\">{data.Password}</span> </p>", data.BusinessEmail, "Login");
                }
            }

            return Ok(result);
        }
    }
}

