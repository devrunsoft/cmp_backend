using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers._Base
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAppApiController : Controller
    {
        protected readonly IMediator _mediator;

        public BaseAppApiController(IMediator mediator)
        {
            _mediator = mediator;

        }



        protected string userName => Request.HttpContext.User.Identity.Name;

    }
}


