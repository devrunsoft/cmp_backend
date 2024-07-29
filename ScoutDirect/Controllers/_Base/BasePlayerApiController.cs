using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScoutDirect.Api.Controllers._Base
{
    public class BasePlayerApiController : BaseApiController
    {
        public BasePlayerApiController(IMediator mediator) : base(mediator)
        {
        }

        protected long rPlayerId => long.Parse(Request.HttpContext.User.FindFirstValue("PlayerId"));
    }
}

