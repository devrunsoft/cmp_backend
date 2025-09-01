using CMPNatural.Application;
using CMPNatural.Application.Model;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Api.Controllers.Admin.Provider
{
    public class DriverController : BaseAdminProviderApiController
    {
        private readonly IWebHostEnvironment Environment;
        public DriverController(IMediator mediator, IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment _environment) : base(mediator, httpContextAccessor)
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
                Id= Id,
                ProviderId = rProviderId,
            });
            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
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
                BaseVirtualPath = wwwPath,
                Email = request.Email,
                IsDefault = request.IsDefault

            });

            if (result.IsSucces())
            {
                sendEmail("Your Driver Account Credentials", $"<p style=\"margin: 5px 0;\"> <strong>Username/Email:</strong> " +
                    $"<span style=\"color: #16a085; font-family: monospace;\">{result.Data.Email}</span> </p> <p style=\"margin: 5px 0;\"> " +
                    $"<strong>Password:</strong> <span style=\"color: #c0392b; font-family: monospace;\">{result.Data.Password}</span> </p>", result.Data.Email, "Login");
            }

            return Ok(result);
        }

        [RequestSizeLimit(100_000_000)]
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
                BaseVirtualPath = wwwPath,
                Email = request.Email,
                IsDefault = request.IsDefault
            });
            return Ok(result);
        }

    }
}

