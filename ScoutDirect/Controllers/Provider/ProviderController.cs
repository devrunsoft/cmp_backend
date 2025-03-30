using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Invoice;
using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using ScoutDirect.Api.Controllers._Base;


namespace CMPNatural.Api.Controllers.Admin.Provider
{

    [Route("api/provider/provider")]
    public class AppProviderController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public AppProviderController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new AdminGetProviderCommand() { Id = rProviderId });
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPutProviderCommand()
            {
                Id = rProviderId,
                Status = input.Status,
                Address = input.Address,
                City = input.City,
                County = input.County,
                Lat = input.Lat,
                Long = input.Long,
                Name = input.Name,
                Rating = input.Rating,
                BusinessLicense = input.BusinessLicense,
                BusinessLicenseExp = input.BusinessLicenseExp,
                EPACompliance = input.EPACompliance,
                EPAComplianceExp = input.EPAComplianceExp,
                HealthDepartmentPermit = input.HealthDepartmentPermit,
                HealthDepartmentPermitExp = input.HealthDepartmentPermitExp,
                Insurance = input.Insurance,
                InsuranceExp = input.InsuranceExp,
                WasteHaulerPermit = input.WasteHaulerPermit,
                BaseVirtualPath = wwwPath,
                AreaLocation = input.AreaLocation,
                ProductIds = input.ProductIds
            });

            return Ok(result);
        }
    }
}

