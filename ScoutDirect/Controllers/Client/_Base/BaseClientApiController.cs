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
    [Route("api/[controller]")]
    [ApiController]
    public class BaseClientApiController : BaseApiController
    {
        protected readonly IMediator _mediator;

        public BaseClientApiController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;

        }

        protected long rCompanyId => long.Parse(Request.HttpContext.User.FindFirstValue("CompanyId"));
        protected string rBusinessEmail => (Request.HttpContext.User.FindFirstValue("businessEmail"));

        protected string userName => Request.HttpContext.User.Identity.Name;

    }
}
