using CMPNatural.Application;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderContractController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ProviderContractController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ProviderGetPaginateProviderContractCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetById([FromRoute] long Id)
        {
            var result = await _mediator.Send(new ProviderGetProviderContractCommand() { Id = Id , ProviderId = rProviderId });
            return Ok(result);
        }

        [HttpPut("sign/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sign([FromRoute] long Id, [FromBody] SignCompanyContractCommand command)
        {
            var result = await _mediator.Send(new SignProviderContractCommand() { ProviderId = rProviderId, ProviderContractId = Id, Sign = command.Sign });

            if (result.IsSucces())
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.ClientHasSigned, result.Data.Id);
                sendEmailToAdmin(emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
                //sendNote(MessageNoteType.ContractSignedByClient, result.Data.OperationalAddressId, result.Data.NoteTitle);
            }

            return Ok(result);
        }
    }
}

