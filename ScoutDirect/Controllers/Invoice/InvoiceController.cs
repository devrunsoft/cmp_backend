using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Command;
using CmpNatural.CrmManagment.Contact;
using CmpNatural.CrmManagment.Invoice;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Application.Handlers;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Invoice
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : CmpBaseController
    {

        private InvoiceApi _invoiceApi;
        private ProductPriceApi _productPriceApi;
        private ProductListApi _productApi;
        private ContactApi _contactApi;
        public InvoiceController(IMediator mediator, InvoiceApi invoiceApi, ProductPriceApi productPriceApi, ProductListApi productApi, ContactApi contactApi) : base(mediator)
        {
            _invoiceApi = invoiceApi;
            _productPriceApi = productPriceApi;
            _productApi = productApi;
            _contactApi = contactApi;
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
        [HttpGet("CheckPayment/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckPayment([FromRoute] long Id)
        {

            var resultdata = await _mediator.Send(new GetInvoiceByIdCommand()
            {
                CompanyId = rCompanyId,
                Id = Id
            });

            var invoice = _invoiceApi.GetInvoice(resultdata.Data.InvoiceId);

           var result= await _mediator.Send(new SentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = resultdata.Data.Id,
                Status = invoice.Data.status
            });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] List<ServiceAppointmentInput> request)
        {
            var resultPrice = request.Select(e =>
                   _productPriceApi.GetById(e.ServiceId, e.ServicePriceId).Data
                  ).ToList();

            //var resultPrice = _productPriceApi.GetById(request.ServiceId, request.ServicePriceId).Data;

            var resultContact = _contactApi.getAlllContact(rBusinessEmail).Data.FirstOrDefault();
            var invoiceNumber = Guid.NewGuid();

            var lst = resultPrice.Select(e =>
              new ProductItemCommand()
              {
                  amount = double.Parse(e.amount),
                  currency = e.currency,
                  name = _productApi.GetById(e.product).Data.name + " - " + e.name,
                  priceId = e._id,
                  productId = e.product,
                  qty = 1,
              }).ToList();

            var command = new CreateInvoiceApiCommand
            {
                dueDate = DateOnly.FromDateTime(DateTime.Now),
                issueDate = DateOnly.FromDateTime(DateTime.Now),
                currency = "USD",
                invoiceNumber = invoiceNumber.ToString(),
                contactDetails = new ContactDetailsCommand { name = rBusinessEmail, email = rBusinessEmail, id = resultContact.id },
                sentTo = new SendTo() { email = new List<string>() { rBusinessEmail } },
                name = rBusinessEmail,
                businessDetails = new BusinessDetailsCommand { name = rBusinessEmail },
                items = lst
            };

            var resultInvoceApi = _invoiceApi.CreateInvoice(command).Data;



            var result = await _mediator.Send(new CreateInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceCrmId = invoiceNumber.ToString(),
                InvoiceNumber = invoiceNumber,
                InvoiceId = resultInvoceApi._id,
                Services = request,
                Amount = lst.Sum(x => x.amount)
            }); ;

            return Ok(result);
        }



        [HttpPut("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long InvoiceId)
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

            var resultInvoice = _invoiceApi.SendInvoice(resultInvoie.Data.InvoiceId,
                new SendInvoiceCommand() { sentTo = new SentTo() { email = new List<string>() { rBusinessEmail } } }
                );

            if (!resultInvoice.IsSucces())
            {
                return Ok(resultInvoice);
            }

            await _mediator.Send(new SentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = InvoiceId,
                Status = ServiceStatus.sent.ToString()
            });

            return Ok(resultInvoice);


        }

    }
}

