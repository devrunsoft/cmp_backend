using System;
using System.Collections.Generic;
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
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllInvoiceCommand()
            {
                CompanyId = rCompanyId
            });
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

        [HttpGet("Request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetRequest()
        {
            var result = await _mediator.Send(new GetAllInvoiceRequestCommand()
            {
                CompanyId = rCompanyId
            });
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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] List<ServiceAppointmentInput> data)
        {
            RegisterInvoiceService service = new RegisterInvoiceService(_mediator, rCompanyId);
            var result = await service.call();
            foreach (var item in result)
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminInvoices, item.Data.InvoiceId);
                 sendEmailToAdmin(emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern, emailDetails.ButtonText);
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

