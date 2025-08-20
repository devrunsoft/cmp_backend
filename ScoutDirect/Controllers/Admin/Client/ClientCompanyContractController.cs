using System;
using CMPNatural.Application;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientCompanyContractController : BaseAdminClientApiController
    {
        public ClientCompanyContractController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
        }

        [HttpGet("OperationalAddress/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetOpr([FromRoute] long OperationalAddressId , [FromQuery] GetAllCompanyContractCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("sign/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sign([FromRoute] long Id, [FromBody] SignCompanyContractCommand command)
        {
            var result = await _mediator.Send(new SignCompanyContractCommand() { CompanyId = rCompanyId, CompanyContractId = Id, Sign = command.Sign });

            if (result.IsSucces())
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.ClientHasSigned, result.Data.Id);
                sendEmailToAdmin(emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
                sendNote(MessageNoteType.ContractSignedByClient, rCompanyId , result.Data.NoteTitle);
            }

            return Ok(result);
        }
    }
}
