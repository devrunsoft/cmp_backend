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
using CMPNatural.Application;
using CMPNatural.Application.Commands.Company;
using CMPNatural.Application.Commands.Invoice;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientInvoicesController : BaseAdminClientApiController
    {
        private InvoiceApi _invoiceApi;
        private ProductPriceApi _productPriceApi;
        private ProductListApi _productApi;
        private ContactApi _contactApi;
        private CustomValueApi _customValueApi;
        public ClientInvoicesController(IMediator mediator, IHttpContextAccessor httpContextAccessor,
            InvoiceApi invoiceApi, ProductPriceApi productPriceApi, ProductListApi productApi, ContactApi contactApi, CustomValueApi customValueApi) : base(mediator, httpContextAccessor)
        {
            _invoiceApi = invoiceApi;
            _productPriceApi = productPriceApi;
            _productApi = productApi;
            _contactApi = contactApi;
            _customValueApi = customValueApi;
        }

        //TODO remove
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

        [HttpGet("OperationalAddress/{operationalAddressId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> AllByOperationalAddressId([FromRoute] long OperationalAddressId, [FromQuery] GetAllInvoiceCommand command)
        {
            command.CompanyId = rCompanyId;
            command.OperationalAddressId = OperationalAddressId;
            var result = await _mediator.Send(command);
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

        [HttpPost("CancelRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> CancelRequest([FromBody] ClientCancelRequestCommand Command)
        {
            Command.CompanyId = rCompanyId;
            var result = await _mediator.Send(Command);
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


        [HttpPost("Pay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Pay([FromQuery] long invoiceNumber)
        {

            var resultdata = await _mediator.Send(new GetInvoiceByInvoiceNumberCommand()
            {
                CompanyId = rCompanyId,
                invoiceId = invoiceNumber
            });

            var invoice = _invoiceApi.GetInvoice(resultdata.Data.InvoiceId);

            var result = await _mediator.Send(new SentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = resultdata.Data.Id,
                //Status = invoice.Data.status
            });

            return Ok(result);
        }

        [HttpPost("Sent/{invoiceNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Sent([FromRoute] long invoiceNumber)
        {

            var resultdata = await _mediator.Send(new GetInvoiceByInvoiceNumberCommand()
            {
                CompanyId = rCompanyId,
                invoiceId = invoiceNumber
            });

            var result = await _mediator.Send(new SentInvoiceCommand()
            {
                CompanyId = rCompanyId,
                InvoiceId = resultdata.Data.Id,
            });

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

