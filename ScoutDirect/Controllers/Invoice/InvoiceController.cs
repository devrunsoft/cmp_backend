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
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Application.Commands.ShoppingCard;
using CMPNatural.Application.Handlers;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        [HttpPost("Pay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Pay([FromQuery] string invoiceNumber)
        {

            var resultdata = await _mediator.Send(new GetInvoiceByInvoiceNumberCommand()
            {
                CompanyId = rCompanyId,
                invoiceNumber = invoiceNumber
            });

            var invoice = _invoiceApi.GetInvoice(resultdata.Data.InvoiceId);

            var result = await _mediator.Send(new SentInvoiceCommand()
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
        public async Task<ActionResult> Post([FromBody] List<ServiceAppointmentInput> r)
        {
            var resultShopping= (await _mediator.Send(new GetAllShoppingCardCommand()
            {
                CompanyId = rCompanyId,
            })).Data;


            var company = (await _mediator.Send(new GetCompanyCommand()
            {
                CompanyId = rCompanyId,
            })).Data;

            var oprAddress = (await _mediator.Send(new GetAllOperationalAddressCommand()
            {
                CompanyId = rCompanyId,
            })).Data.Where((e)=>r.Any(p=>p.OperationalAddressId==e.Id));


            var request =  resultShopping.Select((e) => new ServiceAppointmentInput() {
                FrequencyType = e.FrequencyType,
                OperationalAddressId =e.OperationalAddressId,
                ServiceKind= (ServiceKind) e.ServiceKind,
                ServicePriceId=e.ServicePriceCrmId,
                ServiceTypeId= (ServiceType) e.ServiceId,
                StartDate = e.StartDate,
                ServiceCrmId = e.ServiceCrmId,
                LocationCompanyIds=e.LocationCompanyIds.IsNullOrEmpty()? new List<long>() : e.LocationCompanyIds.Split(",").Select((e)=> long.Parse(e)).ToList(),
                qty = e.Qty
            }).ToList();

            var resultPrice = request.Select(e =>
                  new { price =  _productPriceApi.GetById(e.ServiceCrmId, e.ServicePriceId).Data , product = e }
                  ).ToList();

            //var resultPrice = _productPriceApi.GetById(request.ServiceId, request.ServicePriceId).Data;

            var resultContact = _contactApi.getAlllContact(rBusinessEmail).Data.FirstOrDefault();
            var invoiceNumber = Guid.NewGuid();

            var lst = resultPrice.Select(e =>
              new ProductItemCommand()
              {
                  amount = double.Parse(e.price.amount),
                  currency = e.price.currency,
                  name = _productApi.GetById(e.price.product).Data.name + " - " + e.price.name,
                  priceId = e.price._id,
                  productId = e.price.product,
                  qty = e.product.qty,
              }).ToList();

            var command = new CreateInvoiceApiCommand
            {
                dueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                issueDate = DateOnly.FromDateTime(DateTime.Now),
                currency = "USD",
                invoiceNumber = invoiceNumber.ToString(),
                //
                contactDetails = new ContactDetailsCommand {
                    name = company.PrimaryFirstName + " " +company.PrimaryLastName,
                    //email = rBusinessEmail,
                    phoneNo = company.PrimaryPhonNumber,
                    id = resultContact.id,
                    companyName = company.CompanyName,
                    address = new Address() {
                        addressLine1 = string.Join(" - ", oprAddress.Select((p => "address: " + p.Address)))
                    }

                },
                //
                sentTo = new SendTo() {
                    email = new List<string>() { rBusinessEmail }
                },
                name = company.PrimaryFirstName + " " + company.PrimaryLastName,
                //
                businessDetails = new BusinessDetailsCommand {
                    name  = company.SecondaryFirstName + " " + company.SecondaryFirstName,
                    phoneNo= company.SecondaryPhoneNumber,
                    //customValues= new List<string>() { "string" }
                },
                //
                items = lst,
            };

            var resultInvoceApi = _invoiceApi.CreateInvoice(command).Data;

            var result = await _mediator.Send(new CreateInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceCrmId = invoiceNumber.ToString(),
                InvoiceNumber = invoiceNumber,
                InvoiceId = resultInvoceApi._id,
                Services = request,
                Amount = lst.Sum(x => x.amount * x.qty),

            });


            await _mediator.Send(new DeleteAllShoppingCardCommand()
            {
                CompanyId = rCompanyId,
            });

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

            var resultInvoice = _invoiceApi.DeleteInvoice(resultInvoie.Data.InvoiceId,
                new DeleteInvoiceGoCommand()
                );

            //if (!resultInvoice.IsSucces())
            //{
            //    return Ok(resultInvoice);
            //}

            await _mediator.Send(new DeleteInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = InvoiceId,
            });

            return Ok(resultInvoice);


        }

    }
}

