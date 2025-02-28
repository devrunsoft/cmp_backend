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
using CMPNatural.Api.Controllers._Base;
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
using ScoutDirect.Application.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        private CustomValueApi _customValueApi;
        public InvoiceController(IMediator mediator, InvoiceApi invoiceApi, ProductPriceApi productPriceApi,
            ProductListApi productApi, ContactApi contactApi, CustomValueApi customValueApi) : base(mediator)
        {
            _invoiceApi = invoiceApi;
            _productPriceApi = productPriceApi;
            _productApi = productApi;
            _contactApi = contactApi;
            _customValueApi = customValueApi;
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

            System.Enum.TryParse(invoice.Data.status, out InvoiceStatus invoiceStatus);
            var result= await _mediator.Send(new SentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = resultdata.Data.Id,
                Status = invoiceStatus
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

            System.Enum.TryParse(invoice.Data.status, out InvoiceStatus invoiceStatus);
            var result = await _mediator.Send(new SentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = resultdata.Data.Id,
                Status = invoiceStatus
            });

            return Ok(result);
        }

        [HttpPost("Sent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sent([FromQuery] string invoiceNumber)
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
                Status = InvoiceStatus.Processing
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
                Status = InvoiceStatus.Processing
            });

            return Ok(resultInvoice);


        }

        [NonAction]
        double getMinimumPrice(bool isGreas , List<CustomValueResponse> lst)
        {
            double amount = 0;
            string fieldKey = "";

            if (isGreas)
            {
                fieldKey = "{{ custom_values.minimum_cost_for_grease_trap_management }}";
            }
            else
            {
                fieldKey = "{{ custom_values.minimum_cost_of_cooking_oil_pick_up }}";
            }

               var greasCustomValue= lst.Where(p => p.fieldKey == fieldKey).FirstOrDefault();

                if (greasCustomValue == null)
                {
                    return amount;
                }

              return double.Parse(greasCustomValue.value);

        }

        [NonAction]
        CreateInvoiceApiCommand createInvoceCommand(string invoiceNumber,
            CompanyResponse company,
            ContactResponse resultContact ,
            IEnumerable<OperationalAddress> oprAddress,
            List<ProductItemCommand> lst)
        {
            var command = new CreateInvoiceApiCommand
            {
                dueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
                issueDate = DateOnly.FromDateTime(DateTime.Now),
                currency = "USD",
                invoiceNumber = invoiceNumber.ToString(),
                //
                contactDetails = new ContactDetailsCommand
                {
                    name = company.PrimaryFirstName + " " + company.PrimaryLastName,
                    email = rBusinessEmail,
                    phoneNo = company.PrimaryPhonNumber,
                    id = resultContact.id,
                    companyName = company.CompanyName,
                    address = new Address()
                    {
                        addressLine1 = string.Join(" - ", oprAddress.Select((p => "address: " + p.Address)))
                    }

                },
                //
                sentTo = new SendTo()
                {
                    email = new List<string>() { rBusinessEmail }
                },
                name = company.PrimaryFirstName + " " + company.PrimaryLastName,
                //
                businessDetails = new BusinessDetailsCommand
                {
                    name = company.SecondaryFirstName + " " + company.SecondaryFirstName,
                    phoneNo = company.SecondaryPhoneNumber,
                    //customValues= new List<string>() { "string" }
                },
                //
                items = lst,
            };

            return command;
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


            //var resultGet = _invoiceApi.GetInvoice(resultInvoie.Data.InvoiceId
            // );

            //var resultUpdate = _invoiceApi.Update(resultInvoie.Data.InvoiceId,
            //  resultGet.Data
            // );


            var resultInvoice = _invoiceApi.DeleteInvoice(resultInvoie.Data.InvoiceId,
                new DeleteInvoiceGoCommand()
                );

            //if (!resultInvoice.IsSucces())
            //{
            //    return Ok(resultInvoice);
            //}

            var result = await _mediator.Send(new DeleteInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = InvoiceId,
            });

            return Ok(result);


        }

    }
}

