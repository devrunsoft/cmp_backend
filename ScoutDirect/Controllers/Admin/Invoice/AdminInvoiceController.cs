using CMPNatural.Api.Service;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin.Invoice
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminInvoiceController : BaseAdminApiController
    {
        public AdminInvoiceController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("report")]
        [MenuAuthorize(MenuEnum.Home)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetReport()
        {
            var result = await _mediator.Send(new AdminGetReportCommand() { });
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("Requests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAllRequest([FromQuery] AdminGetAllRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("CreatedInvoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAllInvoice([FromQuery] AdminGetAllCreatedInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long InvoiceId)
        {
            var result = await _mediator.Send(new AdminGetInvoiceCommand() { InvoiceId = InvoiceId });

            return Ok(result);
        }

        [HttpGet("{InvoiceId}/Provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckInvoiceProvider([FromRoute] long InvoiceId)
        {
            var result = await _mediator.Send(new AdminCheckInvoiceProviderCommand() { InvoiceId = InvoiceId });
            return Ok(result);
        }

        [HttpPost("Provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> setInvoiceProvider([FromBody] AdminSetInvoiceProviderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //[HttpGet("OtherLocation/{LocationId}/Provider")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[EnableCors("AllowOrigin")]
        //public async Task<ActionResult> CheckLocationProvider([FromRoute] long LocationId)
        //{
        //    var result = await _mediator.Send(new AdminCheckLocationProviderCommand() { LocationComanyId = LocationId });
        //    return Ok(result);
        //}

        [HttpGet("OprLocation/{OperationalAddressId}/{ProductId}/Provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckOprLocationProvider([FromRoute] long OperationalAddressId , [FromRoute] long ProductId)
        {
            var result = await _mediator.Send(new AdminCheckOprLocationProviderCommand() {
                OperationalAddressId = OperationalAddressId,
                ProductId = ProductId });
            return Ok(result);
        }


        [HttpPost("OprLocation/Provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> setOprLocationProvider([FromBody] AdminSetOprLocationProviderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost("{clientId}/Sent/{invoiceNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sent([FromRoute] long invoiceNumber, [FromRoute] int clientId)
        {
            var rCompanyId = clientId;
            var resultdata = await _mediator.Send(new GetInvoiceByInvoiceNumberCommand()
            {
                CompanyId = rCompanyId,
                invoiceId = invoiceNumber
            });

            var result = await _mediator.Send(new AdminSentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = resultdata.Data.Id,
                //Status = InvoiceStatus.Processing
            });

            if (result.Data.ContractId != null)
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                 sendEmailToClient(rCompanyId ,emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }

            return Ok(result);
        }

        [HttpPost("{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long clientId)
        {
            var rCompanyId = clientId;
            RegisterInvoiceService service = new RegisterInvoiceService(_mediator, rCompanyId);
             var result= await service.call();
            return Ok(new Success<object>() { Data = result });

        }

        [HttpPut("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long InvoiceId , [FromBody] AdminUpdateInvoiceCommand command) {

            command.InvoiceId = InvoiceId;
            var result = await _mediator.Send(command);

            if (result.Data.ContractId != null)
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                sendEmailToClient(result.Data.CompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }
            return Ok(result);
        }


        [HttpPut("Pay/{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Pay([FromRoute] long InvoiceId)
        {
            var result = await _mediator.Send(new AdminInvoicePayCommand() { InvoiceId = InvoiceId });
            return Ok(result);
        }

        [HttpPut("UpdateComplete/{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> UpdateCompletePut([FromRoute] long InvoiceId, [FromBody] AdminProviderUpdateInvoiceCommand command)
        {

            command.InvoiceId = InvoiceId;
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
        public async Task<ActionResult> SubmitCompletePut([FromRoute] long InvoiceId, [FromBody] AdminProviderSubmitInvoiceCommand command)
        {

            command.InvoiceId = InvoiceId;
            var result = await _mediator.Send(command);

            if (result.IsSucces())
            {
                //var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                //sendEmailToClient(result.Data.CompanyId, emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
            }
            return Ok(result);
        }

        [HttpDelete("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long InvoiceId)
        {

            var result = await _mediator.Send(new AdminCancelInvoiceCommand()
            {
                InvoiceId = InvoiceId,
            });

            return Ok(result);

        }

    }
}

