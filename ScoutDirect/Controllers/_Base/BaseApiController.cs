using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScoutDirect.Api.Controllers._Base
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : Controller
    {
        protected readonly IMediator _mediator;

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;

        }

        protected long rCompanyId => long.Parse(Request.HttpContext.User.FindFirstValue("CompanyId"));

        protected string userName => Request.HttpContext.User.Identity.Name;

    }
}

