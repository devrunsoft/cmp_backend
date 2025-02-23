using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Application;
using CMPNatural.Application.Model.ServiceAppointment;
using CmpNatural.CrmManagment.Product;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Client
{
    public class ClientShoppingCardController : BaseAdminClientApiController
    {
        private ProductListApi _api;
        private ProductPriceApi _priceapi;
        public ClientShoppingCardController(IMediator mediator, ProductListApi api, ProductPriceApi priceapi, IHttpContextAccessor httpContextAccessor)
            : base(mediator, httpContextAccessor)
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
                //ServiceCrmId = request.ServiceCrmId,
                ServiceTypeId = request.ServiceTypeId,
                //ServicePriceId = request.ServicePriceId,
                ServiceKind = request.ServiceKind,
                LocationCompanyIds = request.LocationCompanyIds,
                qty = request.qty,
                ProductId = request.ProductId,
                ProductPriceId = request.ProductPriceId
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

