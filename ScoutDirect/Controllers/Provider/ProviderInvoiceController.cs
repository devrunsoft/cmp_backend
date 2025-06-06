using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Provider
{
    public class ProviderInvoiceController : BaseProviderApiController
    {
        public ProviderInvoiceController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromQuery] ProviderGetAllInvoiceCommand command)
        {
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long InvoiceId)
        {
            var result = await _mediator.Send(new ProviderGetInvoiceCommand() { InvoiceId = InvoiceId, ProviderId = rProviderId });
            return Ok(result);
        }

        [HttpPut("UpdateComplete/{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UpdateCompletePut([FromRoute] long InvoiceId, [FromBody] ProviderUpdateInvoiceCommand command)
        {

            command.InvoiceId = InvoiceId;
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                //var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                //sendEmailToClient(result.Data.CompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }
            return Ok(result);
        }

        [HttpPut("SubmitComplete/{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> SubmitCompletePut([FromRoute] long InvoiceId, [FromBody] ProviderSubmitInvoiceCommand command)
        {

            command.InvoiceId = InvoiceId;
            command.ProviderId = rProviderId;
            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                //var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                //sendEmailToClient(result.Data.CompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }
            return Ok(result);
        }
    }
}

