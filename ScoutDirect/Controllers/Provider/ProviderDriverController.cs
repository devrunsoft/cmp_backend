using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    public class ProviderDriverController : BaseProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public ProviderDriverController(IMediator mediator,
            IWebHostEnvironment _environment) : base(mediator)
        {
            Environment = _environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Get()
        {
            var result = await _mediator.Send(new GetAllDriverCommand()
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
            var result = await _mediator.Send(new GetDriverCommand()
            {
                Id = Id,
                ProviderId = rProviderId,
            });
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Post([FromForm] DriverInput request)
        {
            string wwwPath = Environment.ContentRootPath;

            var result = await _mediator.Send(new AddDriverCommand()
            {
                License = request.License,
                LicenseExp = request.LicenseExp,
                BackgroundCheck = request.BackgroundCheck,
                BackgroundCheckExp = request.BackgroundCheckExp,
                ProfilePhoto = request.ProfilePhoto,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProviderId = rProviderId,
                BaseVirtualPath = wwwPath
            });
            return Ok(result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult> Put([FromRoute] long Id, [FromForm] DriverInput request)
        {
            string wwwPath = Environment.ContentRootPath;
            var result = await _mediator.Send(new EditDriverCommand()
            {
                Id = Id,
                License = request.License,
                LicenseExp = request.LicenseExp,
                BackgroundCheck = request.BackgroundCheck,
                BackgroundCheckExp = request.BackgroundCheckExp,
                ProfilePhoto = request.ProfilePhoto,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProviderId = rProviderId,
                BaseVirtualPath = wwwPath
            });
            return Ok(result);
        }

    }
}

