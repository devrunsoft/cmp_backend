using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Api.Controllers._Base;
using CMPNatural.Application;
using CMPNatural.Application.Model.ServiceAppointment;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.shoppingcard
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCardController : CmpBaseController
    {
        private ProductListApi _api;
        private ProductPriceApi _priceapi;
        public ShoppingCardController(IMediator mediator, ProductListApi api, ProductPriceApi priceapi) : base(mediator)
        {
            _api = api;
            _priceapi = priceapi;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllShoppingCardCommand()
            {
                CompanyId = rCompanyId
            });
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromBody] ServiceAppointmentInput request)
        {
            var product = _api.GetById(request.ServiceId);

            var price = _priceapi.GetById(request.ServiceId, request.ServicePriceId);

            var address = await _mediator.Send(new GetByIdServiceOperationalAddressCommand()
            { Id=request.OperationalAddressId,CompanyId=rCompanyId});

            var result = await _mediator.Send(new AddServiceShoppingCardCommand()
            {
                CompanyId = rCompanyId,
                FrequencyType = request.FrequencyType,
                StartDate = request.StartDate,
                OperationalAddressId = request.OperationalAddressId,
                ServiceId = request.ServiceId,
                ServicePriceId = request.ServicePriceId,
                Name= product.Data.name,
                AddressName = address.Data.Name,
                PriceName = price.Data.name,
                ServiceKind =request.ServiceKind
            });
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            var result = await _mediator.Send(new DeleteShoppingCardCommand()
            {
                CompanyId = rCompanyId,
                Id = Id
            });
            return Ok(result);
        }
    }
}

