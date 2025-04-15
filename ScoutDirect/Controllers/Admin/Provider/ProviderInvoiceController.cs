using ScoutDirect.Application.Responses;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    public class ProviderInvoiceController : BaseAdminProviderApiController
    {
        public ProviderInvoiceController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(mediator, httpContextAccessor)
        {
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

