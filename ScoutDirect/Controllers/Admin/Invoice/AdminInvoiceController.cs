using System;
using CmpNatural.CrmManagment.Api.CustomValue;
using CmpNatural.CrmManagment.Command;
using CmpNatural.CrmManagment.Contact;
using CmpNatural.CrmManagment.Invoice;
using CmpNatural.CrmManagment.Model;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Application.Commands.ShoppingCard;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI;

namespace CMPNatural.Api.Controllers.Admin.Invoice
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class AdminInvoiceController : BaseAdminApiController
    {
        private InvoiceApi _invoiceApi;
        private ProductPriceApi _productPriceApi;
        private ProductListApi _productApi;
        private ContactApi _contactApi;
        private CustomValueApi _customValueApi;
        public AdminInvoiceController(IMediator mediator, InvoiceApi invoiceApi, ProductPriceApi productPriceApi,
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
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{InvoiceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get([FromRoute] string InvoiceId)
        {
            var result = await _mediator.Send(new AdminGetInvoiceCommand() { InvoiceId = InvoiceId });

            return Ok(result);
        }



        [HttpPost("Sent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sent([FromQuery] string invoiceNumber)
        {

            var resultdata = await _mediator.Send(new AdminGetInvoiceNumberCommand()
            {
                invoiceNumber = invoiceNumber
            });

             _invoiceApi.GetInvoice(resultdata.Data.InvoiceId);

            var result = await _mediator.Send(new AdminSentInvoiceCommand()
            {
                InvoiceId = resultdata.Data.Id,
                Status = ServiceStatus.sent.GetDescription()
            });
            return Ok(result);
        }

        [HttpPost("{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] List<ServiceAppointmentInput> r, [FromRoute] long clientId)
        {
            var rCompanyId = clientId;

            var resultShopping = (await _mediator.Send(new GetAllShoppingCardCommand()
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
            })).Data.Where((e) => r.Any(p => p.OperationalAddressId == e.Id));


            var request = resultShopping.Select((e) => new ServiceAppointmentInput()
            {
                FrequencyType = e.FrequencyType,
                OperationalAddressId = e.OperationalAddressId,
                ServiceKind = (ServiceKind)e.ServiceKind,
                //ServicePriceId=e.ServicePriceCrmId,
                ServiceTypeId = (ServiceType)e.ServiceId,
                StartDate = e.StartDate,
                //ServiceCrmId = e.ServiceCrmId,
                ProductPriceId = e.ProductPriceId.Value,
                ProductId = e.ProductId.Value,
                LocationCompanyIds = e.LocationCompanyIds.IsNullOrEmpty() ? new List<long>() : e.LocationCompanyIds.Split(",").Select((e) => long.Parse(e)).ToList(),
                qty = e.Qty
            }).ToList();


            var invoiceId = Guid.NewGuid();

            var result = await _mediator.Send(new CreateInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceCrmId = invoiceId.ToString(),
                InvoiceNumber = invoiceId,
                InvoiceId = invoiceId.ToString(),
                Services = request,
                Amount = 0,
            });


            await _mediator.Send(new DeleteAllShoppingCardCommand()
            {
                CompanyId = rCompanyId,
            });

            return Ok(result);
        }

        [NonAction]
        double getMinimumPrice(bool isGreas, List<CustomValueResponse> lst)
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

            var greasCustomValue = lst.Where(p => p.fieldKey == fieldKey).FirstOrDefault();

            if (greasCustomValue == null)
            {
                return amount;
            }

            return double.Parse(greasCustomValue.value);

        }


        [NonAction]
        CreateInvoiceApiCommand createInvoceCommand(string invoiceNumber,
            CompanyResponse company,
            ContactResponse resultContact,
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
                    email = company.BusinessEmail,
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
                    email = new List<string>() { company.BusinessEmail }
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



    }
}

