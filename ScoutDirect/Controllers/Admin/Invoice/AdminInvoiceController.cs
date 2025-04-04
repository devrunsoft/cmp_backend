using System;
using System.Data;
using CmpNatural.CrmManagment.Api.CustomValue;
using CmpNatural.CrmManagment.Command;
using CmpNatural.CrmManagment.Contact;
using CmpNatural.CrmManagment.Invoice;
using CmpNatural.CrmManagment.Model;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Api.Service;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Application.Commands.ShoppingCard;
using CMPNatural.Application.Model;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<ActionResult> Get([FromRoute] string InvoiceId)
        {
            var result = await _mediator.Send(new AdminGetInvoiceCommand() { InvoiceId = InvoiceId });

            return Ok(result);
        }

        [HttpGet("{InvoiceId}/Provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CheckInvoiceProvider([FromRoute] string InvoiceId)
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
        public async Task<ActionResult> Sent([FromRoute] string invoiceNumber, [FromRoute] int clientId)
        {
            var rCompanyId = clientId;
            var resultdata = await _mediator.Send(new GetInvoiceByInvoiceNumberCommand()
            {
                CompanyId = rCompanyId,
                invoiceNumber = invoiceNumber
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
                 sendEmailToClient(rCompanyId ,emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern);
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
        public async Task<ActionResult> Put([FromRoute] string InvoiceId , [FromBody] AdminUpdateInvoiceCommand command) {

            command.InvoiceId = InvoiceId;
            var result = await _mediator.Send(command);

            if (result.Data.ContractId != null)
            {
                var emailDetails = EmailLinkHelper.GetEmailDetails(EmailLinkEnum.AdminHasCreateContract, result.Data.ContractId.Value);
                 sendEmailToAdmin(emailDetails.Subject, emailDetails.Body, emailDetails.LinkPattern);
            }
            return Ok(result);

        }

        //[NonAction]
        //double getMinimumPrice(bool isGreas, List<CustomValueResponse> lst)
        //{
        //    double amount = 0;
        //    string fieldKey = "";

        //    if (isGreas)
        //    {
        //        fieldKey = "{{ custom_values.minimum_cost_for_grease_trap_management }}";
        //    }
        //    else
        //    {
        //        fieldKey = "{{ custom_values.minimum_cost_of_cooking_oil_pick_up }}";
        //    }

        //    var greasCustomValue = lst.Where(p => p.fieldKey == fieldKey).FirstOrDefault();

        //    if (greasCustomValue == null)
        //    {
        //        return amount;
        //    }

        //    return double.Parse(greasCustomValue.value);

        //}


        //[NonAction]
        //CreateInvoiceApiCommand createInvoceCommand(string invoiceNumber,
        //    CompanyResponse company,
        //    ContactResponse resultContact,
        //    IEnumerable<OperationalAddress> oprAddress,
        //    List<ProductItemCommand> lst)
        //{
        //    var command = new CreateInvoiceApiCommand
        //    {
        //        dueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10)),
        //        issueDate = DateOnly.FromDateTime(DateTime.Now),
        //        currency = "USD",
        //        invoiceNumber = invoiceNumber.ToString(),
        //        //
        //        contactDetails = new ContactDetailsCommand
        //        {
        //            name = company.PrimaryFirstName + " " + company.PrimaryLastName,
        //            email = company.BusinessEmail,
        //            phoneNo = company.PrimaryPhonNumber,
        //            id = resultContact.id,
        //            companyName = company.CompanyName,
        //            address = new Address()
        //            {
        //                addressLine1 = string.Join(" - ", oprAddress.Select((p => "address: " + p.Address)))
        //            }

        //        },
        //        //
        //        sentTo = new SendTo()
        //        {
        //            email = new List<string>() { company.BusinessEmail }
        //        },
        //        name = company.PrimaryFirstName + " " + company.PrimaryLastName,
        //        //
        //        businessDetails = new BusinessDetailsCommand
        //        {
        //            name = company.SecondaryFirstName + " " + company.SecondaryFirstName,
        //            phoneNo = company.SecondaryPhoneNumber,
        //            //customValues= new List<string>() { "string" }
        //        },
        //        //
        //        items = lst,
        //    };

        //    return command;
        //}



    }
}

