using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.Request;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin
{
    [ApiController]
    [MenuAuthorize(MenuEnum.Requests)]
    [Route("api/admin/[controller]")]
    public class AdminRequestController : BaseAdminApiController
    {
        public AdminRequestController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("Requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAllRequest([FromQuery] AdminGetAllRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("{RequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long RequestId)
        {
            var result = await _mediator.Send(new AdminGetRequestCommand() { RequestId = RequestId });

            return Ok(result);
        }

        [HttpPut("{RequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long RequestId, [FromBody] AdminUpdateRequestCommand command)
        {

            command.RequestId = RequestId;
            var result = await _mediator.Send(command);

            if (result.IsSucces() && result.Data.ContractId != null)
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                sendEmailToClient(result.Data.CompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }

            return Ok(result);
        }

        [HttpPost("{clientId}/Sent/{RequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sent([FromRoute] long RequestId, [FromRoute] int clientId)
        {
            var rCompanyId = clientId;
            //var resultdata = await _mediator.Send(new GetInvoiceByInvoiceNumberCommand()
            //{
            //    CompanyId = rCompanyId,
            //    invoiceId = invoiceNumber
            //});

            var result = await _mediator.Send(new AdminSentRequestCommand()
            {
                CompanyId = rCompanyId,
                RequestId = RequestId
                //Status = InvoiceStatus.Processing
            });

            if (result.Data.ContractId != null)
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                sendEmailToClient(rCompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
                sendNote(MessageNoteType.ContractCreate, clientId, result.Data.OperationalAddressId, result.Data.ReqNumber);
            }

            return Ok(result);
        }


        [HttpPut("UpdateComplete/{RequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UpdateCompletePut([FromRoute] long InvoiceId, [FromBody] AdminProviderUpdateRequestCommand command)
        {

            command.RequestId = InvoiceId;
            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                //var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                //sendEmailToClient(result.Data.CompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }
            return Ok(result);
        }

        [HttpPut("SubmitComplete/{RequestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> SubmitCompletePut([FromRoute] long RequestId, [FromBody] AdminProviderSubmitRequestCommand command)
        {

            command.RequestId = RequestId;
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

