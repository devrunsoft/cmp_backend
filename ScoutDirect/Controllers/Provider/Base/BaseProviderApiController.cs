using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoutDirect.Core.Caching;

namespace ScoutDirect.Api.Controllers._Base
{
    [Authorize]
    [Route("api/provider/[controller]")]
    [ApiController]
    public class BaseProviderApiController : BaseApiController
    {
        protected readonly IMediator _mediator;
        public BaseProviderApiController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }
        protected long rProviderId => long.Parse(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        protected long? rDriverId
        {
            get
            {
                var value = Request.HttpContext.User.FindFirst("DriverId")?.Value;

                return long.TryParse(value, out var id)
                    ? id
                    : null;
            }
        }
        protected bool rIsDriver
        {
            get
            {
                var value = Request.HttpContext.User.FindFirst("IsDriver")?.Value;

                return bool.TryParse(value, out var id)
                    ? id
                    : false;
            }
        }

        protected string rEmail => (Request.HttpContext.User.FindFirstValue("Email"));
        protected string userName => Request.HttpContext.User.Identity.Name;
    }
}
