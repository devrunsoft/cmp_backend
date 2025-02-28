using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmpNatural.CrmManagment.Invoice;
using CMPNatural.Application;
using CMPNatural.Application.Commands.Admin.provider;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProviderController : BaseAdminApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ProviderController(IMediator mediator, IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetAll([FromQuery] AdminGetAllProviderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> get([FromRoute] int Id)
        {
            var result = await _mediator.Send(new AdminGetProviderCommand() { Id = Id});
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id , [FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPutProviderCommand()
            {
                Id = Id,
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
                EPACompliance =  input.EPACompliance,
                EPAComplianceExp = input.EPAComplianceExp,
                HealthDepartmentPermit = input.HealthDepartmentPermit,
                HealthDepartmentPermitExp = input.HealthDepartmentPermitExp,
                Insurance = input.Insurance,
                InsuranceExp = input.InsuranceExp,
                WasteHaulerPermit = input.WasteHaulerPermit,
                BaseVirtualPath = wwwPath,
                AreaLocation = input.AreaLocation
            });

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromForm] ProviderInput input)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new AdminPostProviderCommand()
            {
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
                AreaLocation = input.AreaLocation
            });

            return Ok(result);
        }
    }
}

