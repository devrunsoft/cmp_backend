using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.ServiceAppointment
{

    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : CmpBaseController
    {
        private ProductListApi _api;
        private ProductPriceApi _priceapi;

        public ServiceController(IMediator mediator , ProductListApi api, ProductPriceApi priceapi) : base(mediator)
        {
            _api = api;
            _priceapi = priceapi;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = _api.call();

            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetPrice([FromRoute] string Id)
        {
            var result = _priceapi.call(Id);

            return Ok(result);
        }

    }
}

