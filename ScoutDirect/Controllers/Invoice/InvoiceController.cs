//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CmpNatural.CrmManagment.Invoice;
//using CmpNatural.CrmManagment.Product;
//using CMPNatural.Api.Controllers._Base;
//using Google.Protobuf.WellKnownTypes;
//using MediatR;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace CMPNatural.Api.Controllers.Invoice
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class InvoiceController : CmpBaseController
//    {

//        //private CreateInvoiceApi _invoiceApi;
//        public InvoiceController(IMediator mediator) : base(mediator)
//        {
//            _invoiceApi = invoiceApi;
//        }

//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [EnableCors("AllowOrigin")]
//        public async Task<ActionResult> Get()
//        {
//            var result = _invoiceApi.call();

//            return Ok(result);
//        }
//    }
//}

