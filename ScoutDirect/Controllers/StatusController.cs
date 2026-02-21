using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Document;
using CMPNatural.Application.Handlers.CommandHandlers;
using CMPNatural.Application.Responses;
using CMPNatural.Application;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers
{
    [Route("api/[controller]")]
    public class RegisterStatusController : BaseClientApiController
    {
        public RegisterStatusController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            //var resultlocation = await _mediator.Send(new GetLocationCompanyCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (!resultlocation.Data.Any())
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.ProfessionalInformation.ToString() });

            //}
            ////
            //var resultdocument = await _mediator.Send(new GetDocumentCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultdocument.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.DocumentSubmission.ToString() });

            //}

            //var resultbiling = await _mediator.Send(new GetBilingInformationCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultbiling.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.BillingDetails.ToString() });

            //}
            var resultCompany = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (resultCompany.Data == null)
            {
                return Ok(new NoAcess() { Data = null , Success = false });
            }

            if(!((CompanyResponse)resultCompany.Data).Registered)
            {
                return Ok(new Success<String>() { Data = RegisterType.NotActivate.ToString() });
            }

            return Ok(new Success<String>() { Data = RegisterType.Registered.ToString() });
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> RegistartionStatusLogin()
        {

            var resultbiling = await _mediator.Send(new GetBilingInformationCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (resultbiling.Data == null)
            {
                return Ok(new Success<String>() { Data = RegisterType.BillingDetails.ToString() });

            }

            var resultlocation = await _mediator.Send(new GetLocationCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (!resultlocation.Data.Any())
            {
                return Ok(new Success<String>() { Data = RegisterType.ProfessionalInformation.ToString() });

            }
            //
            //var resultdocument = await _mediator.Send(new GetDocumentCommand()
            //{
            //    CompanyId = rCompanyId,
            //})!;

            //if (resultdocument.Data == null)
            //{
            //    return Ok(new Success<String>() { Data = RegisterType.DocumentSubmission.ToString() });

            //}


            var resultCompany = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (!((CompanyResponse)resultCompany.Data).Registered)
            {
                return Ok(new Success<String>() { Data = RegisterType.NotActivate.ToString() });
            }

            return Ok(new Success<String>() { Data = RegisterType.Registered.ToString() });
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> MissingRegistrationFields()
        {
            var missing = new List<string>();

            var resultCompany = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })!;

            if (resultCompany.Data == null)
            {
                return Ok(new NoAcess() { Data = null, Success = false });
            }

            var company = (CompanyResponse)resultCompany.Data;
            if (string.IsNullOrWhiteSpace(company.CompanyName))
                missing.Add("CompanyName");
            if (string.IsNullOrWhiteSpace(company.BusinessEmail))
                missing.Add("Company.BusinessEmail");

            if (string.IsNullOrWhiteSpace(company.PrimaryFirstName))
                missing.Add("PrimaryFirstName");
            if (string.IsNullOrWhiteSpace(company.PrimaryLastName))
                missing.Add("PrimaryLastName");

            long? operationalAddressId = null;
            var claimValue = Request.HttpContext.User.FindFirstValue("OperationalAddressId");
            if (long.TryParse(claimValue, out var parsedOperationalId) && parsedOperationalId > 0)
                operationalAddressId = parsedOperationalId;

            var addressResult = await _mediator.Send(new GetAllOperationalAddressCommand()
            {
                CompanyId = rCompanyId,
                OperationalAddressId = operationalAddressId
            })!;

            var operationalAddress = addressResult.Data?.FirstOrDefault();
            if (operationalAddress == null)
            {
                missing.Add("OperationalAddress.Username/Password");
                missing.Add("OperationalAddress.Address");
                missing.Add("OperationalAddress.Phone");
                missing.Add("OperationalAddress.ContactPerson");
                missing.Add("OperationalAddress.Location");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(operationalAddress.Username))
                    missing.Add("OperationalAddress.Username/Password");
                if (string.IsNullOrWhiteSpace(operationalAddress.Address))
                    missing.Add("OperationalAddress.Address");
                if (string.IsNullOrWhiteSpace(operationalAddress.LocationPhone))
                    missing.Add("OperationalAddress.Phone");
                if (string.IsNullOrWhiteSpace(operationalAddress.FirstName) && string.IsNullOrWhiteSpace(operationalAddress.LastName))
                    missing.Add("OperationalAddress.ContactPerson");
                if (!operationalAddress.Lat.HasValue || !operationalAddress.Long.HasValue)
                    missing.Add("OperationalAddress.Location");
            }

            return Ok(new Success<object>()
            {
                Data = new
                {
                    MissingFields = missing,
                    IsComplete = missing.Count == 0
                }
            });
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> RequestEmailChange([FromBody] RequestCompanyEmailChangeCommand command)
        {
            if (command == null)
                return Ok(new NoAcess() { Message = "Invalid request." });

            command.CompanyId = rCompanyId;
            var result = await _mediator.Send(command);
            if (!result.Success)
                return Ok(result);

            var email = command.Email?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
                return Ok(new NoAcess() { Message = "Email is required." });

            var subject = "Email Change Verification Code";
            var body = $"Your verification code is <strong>{result.Data}</strong>.";
            sendEmail(subject, body, email);

            return Ok(new Success<object>()
            {
                Data = new { Message = "Verification code sent to your email." }
            });
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ConfirmEmailChange([FromBody] ConfirmCompanyEmailChangeCommand command)
        {
            if (command == null)
                return Ok(new NoAcess() { Message = "Invalid request." });

            command.CompanyId = rCompanyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
