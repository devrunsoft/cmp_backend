using System;
using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Provider
{
	public class ProviderVehicleController : BaseProviderApiController
    {

        private readonly IWebHostEnvironment Environment;
        public ProviderVehicleController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllVehicleCommand()
            {
                ProviderId = rProviderId,
            });
            return Ok(result);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> GetById([FromRoute] long Id)
        {
            var result = await _mediator.Send(new GetVehicleCommand()
            {
                Id = Id,
                ProviderId = rProviderId,
            });
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromForm] VehicleInput request)
        {
            string wwwPath = Environment.ContentRootPath;

            var result = await _mediator.Send(new AddVehicleCommand()
            {
                ProviderId = rProviderId,
                Capacity = request.Capacity,
                VehicleRegistration = request.VehicleRegistration,
                VehicleRegistrationExp = request.VehicleRegistrationExp,

                VehicleInsurance = request.VehicleInsurance,
                VehicleInsuranceExp = request.VehicleInsuranceExp,

                InspectionReport = request.InspectionReport,
                InspectionReportExp = request.InspectionReportExp,
                Picture = request.Picture,

                MeasurementCertificate = request.MeasurementCertificate,

                PeriodicVehicleInspections = request.PeriodicVehicleInspections,
                PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp,

                Weight = request.Weight,

                VehicleCompartments = request.VehicleCompartments,
                VehicleService = request.VehicleService,
                Name = request.Name,
                BaseVirtualPath = wwwPath
            });
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromForm] VehicleInput request)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new EditVehicleCommand()
            {
                Id = Id,
                ProviderId = rProviderId,
                Capacity = request.Capacity,
                VehicleRegistration = request.VehicleRegistration,
                VehicleRegistrationExp = request.VehicleRegistrationExp,

                VehicleInsurance = request.VehicleInsurance,
                VehicleInsuranceExp = request.VehicleInsuranceExp,

                InspectionReport = request.InspectionReport,
                InspectionReportExp = request.InspectionReportExp,
                Picture = request.Picture,

                MeasurementCertificate = request.MeasurementCertificate,

                PeriodicVehicleInspections = request.PeriodicVehicleInspections,
                PeriodicVehicleInspectionsExp = request.PeriodicVehicleInspectionsExp,
                Name = request.Name,
                Weight = request.Weight,

                VehicleCompartments = request.VehicleCompartments,
                VehicleService = request.VehicleService,
                BaseVirtualPath = wwwPath
            });
            return Ok(result);
        }

    }
}

