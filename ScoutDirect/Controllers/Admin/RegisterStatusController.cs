using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin
{
    public class RegisterStatusController : BaseAdminApiController
    {
        public RegisterStatusController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Route("[action]/{companyId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> MissingRegistrationFields([FromRoute] long companyId)
        {
            var missing = new List<string>();

            var resultCompany = await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = companyId,
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

            var addressResult = await _mediator.Send(new GetAllOperationalAddressCommand()
            {
                CompanyId = companyId
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
                //if (string.IsNullOrWhiteSpace(operationalAddress.Username))
                //    missing.Add("OperationalAddress.Username/Password");
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
        [Route("[action]/{companyId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> RequestEmailChange([FromBody] RequestCompanyEmailChangeCommand command, [FromRoute] long companyId)
        {
            if (command == null)
                return Ok(new NoAcess() { Message = "Invalid request." });

            command.CompanyId = companyId;
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
        [Route("[action]/{companyId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> ConfirmEmailChange([FromBody] ConfirmCompanyEmailChangeCommand command , [FromRoute] long companyId)
        {
            if (command == null)
                return Ok(new NoAcess() { Message = "Invalid request." });

            command.CompanyId = companyId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
