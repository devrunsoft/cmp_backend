using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Api.CustomValue;
using CmpNatural.CrmManagment.Command;
using CmpNatural.CrmManagment.Contact;
using CmpNatural.CrmManagment.Invoice;
using CmpNatural.CrmManagment.Model;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Api.Service;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Application.Commands.ShoppingCard;
using CMPNatural.Application.Handlers;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Invoice
{
    [ApiController]
 
    public class InvoiceController : BaseClientApiController
    {


        public InvoiceController(IMediator mediator, InvoiceApi invoiceApi) : base(mediator)
        {
        }

        [HttpGet("OperationalAddress/{operationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> AllByOperationalAddressId([FromRoute] long OperationalAddressId , [FromQuery] GetAllInvoiceCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] long InvoiceId)
        {
            var result = await _mediator.Send(new ClientGetInvoiceCommand() { InvoiceId = InvoiceId });
            return Ok(result);
        }

        [HttpPost("CancelRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CancelRequest([FromBody] ClientCancelRequestCommand Command)
        {
            Command.CompanyId = rCompanyId;
            var result = await _mediator.Send(Command);
            return Ok(result);
        }

        [HttpGet("Request/{OperationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetRequest([FromRoute] long OperationalAddressId, [FromQuery] GetAllInvoiceRequestCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetPayableCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetPayableCount()
        {
            var result = await _mediator.Send(new GetAllInvoicePayableCommand()
            {
                CompanyId = rCompanyId
            });
            return Ok(result);
        }

        [HttpPost("Pay/{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Pay([FromRoute] long InvoiceId)
        {
            var result = await _mediator.Send(new ClientInvoicePayCommand() { InvoiceId = InvoiceId });
            return Ok(result);
        }

        [HttpPost("{BillingInformationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromRoute] long BillingInformationId)
        {
            RegisterInvoiceService service = new RegisterInvoiceService(_mediator, rCompanyId);
            var result = await service.call(BillingInformationId);
            foreach (var item in result)
            {
                 var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminInvoices, item.Data.InvoiceId);
                 sendEmailToAdmin(emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
                 sendNote(MessageNoteType.RequestCreateByClient, item.Data.ReqNumber);
            }

            return Ok(new Success<object>());
        }


        [HttpDelete("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long InvoiceId)
        {

            var resultInvoie = await _mediator.Send(new GetInvoiceByIdCommand()
            {
                CompanyId = rCompanyId,
                Id = InvoiceId
            });

            if (!resultInvoie.IsSucces())
            {
                sendNote(MessageNoteType.RequestCanceledByClient , resultInvoie.Data.ReqNumber);
                return Ok(resultInvoie);
            }

            var result = await _mediator.Send(new DeleteInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = InvoiceId,
            });

            return Ok(result);

        }

    }
}

