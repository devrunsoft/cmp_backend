using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Enums;
using Elmah.ContentSyndication;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.CompanyContract
{
    public class CompanyContractController : BaseClientApiController
    {
        public CompanyContractController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("OperationalAddress/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetOpr([FromRoute] long OperationalAddressId, [FromQuery] GetAllCompanyContractCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Signable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Signable()
        {
            var result = await _mediator.Send(new GetAllSiganbleCompanyContractCommand() { CompanyId = rCompanyId });
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetCompanyContractCommand() { CompanyId = rCompanyId  , ContractId = Id});
            return Ok(result);
        }

        [HttpPut("sign/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sign([FromRoute] long Id , [FromBody] SignCompanyContractCommand command)
        {
            var result = await _mediator.Send(new SignCompanyContractCommand() { CompanyId = rCompanyId , CompanyContractId = Id  , Sign = command .Sign});

            if (result.IsSucces())
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.ClientHasSigned, result.Data.Id);
                sendEmailToAdmin(emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
                sendNote(MessageNoteType.ContractSignedByClient, result.Data.NoteTitle);
            }

            return Ok(result);
        }
    }
}

