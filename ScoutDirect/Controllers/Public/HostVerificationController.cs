using CMPNatural.Core.Models;
using CMPNatural.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CMPNatural.Api.Controllers.Public
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/public/[controller]")]
    public class HostVerificationController : ControllerBase
    {
        private readonly IHostVerificationService _hostVerificationService;

        public HostVerificationController(IHostVerificationService hostVerificationService)
        {
            _hostVerificationService = hostVerificationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<HostVerificationResponse> Get([FromQuery] string challenge)
        {
            if (string.IsNullOrWhiteSpace(challenge))
            {
                return BadRequest();
            }

            return Ok(new HostVerificationResponse
            {
                Host = Request.Host.Host.Trim().ToLowerInvariant(),
                Signature = _hostVerificationService.CreateSignature(challenge)
            });
        }
    }
}
