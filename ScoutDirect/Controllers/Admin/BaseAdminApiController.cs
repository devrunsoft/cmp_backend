using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers.Admin
{
    [Authorize(Roles = "SuperAdmin,LimitedAdmin")]
    [ApiController]
    [Route("api/admin/[controller]")]
    public class BaseAdminApiController : BaseApiController
    {
        protected readonly IMediator _mediator;
        public BaseAdminApiController(IMediator mediator): base(mediator)
        {
            _mediator = mediator;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var isAdmin = bool.Parse(context.HttpContext.User.FindFirstValue("isAdmin"));

                if (!isAdmin)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }

            base.OnActionExecuting(context);
        }

        protected Guid PersonId => Guid.Parse(Request.HttpContext.User.FindFirstValue("PersonId"));
        protected long AdminId => long.Parse(Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        protected long Email => long.Parse(Request.HttpContext.User.FindFirstValue("Email"));

    }
}

