using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Application;
using CMPNatural.Application.Model.ServiceAppointment;
using CMPNatural.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;
using ScoutDirect.Application.Responses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.shoppingcard
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCardController : BaseClientApiController
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

            var result = await _mediator.Send(new AddServiceShoppingCardCommand()
            {
                CompanyId = rCompanyId,
                FrequencyType = request.FrequencyType,
                StartDate = request.StartDate,
                OperationalAddressId = request.OperationalAddressId,
                ServiceTypeId = request.ServiceTypeId,
                //Name = "",
                //AddressName = "",
                //Address = "",
                //PriceName = "",
                ProductId = request.ProductId,
                ServiceKind =request.ServiceKind,
                LocationCompanyIds = request.LocationCompanyIds,
                qty=request.qty,
                ProductPriceId = request.ProductPriceId,
                ToHour = request.ToHour,
                FromHour = request.FromHour,
                DayOfWeek = request.DayOfWeek
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

