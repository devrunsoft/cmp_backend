using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScoutDirect.Api.Controllers._Base;

namespace CMPNatural.Api.Controllers._Base
{
    public class BaseRegisteredController : BaseApiController
    {
        public BaseRegisteredController(IMediator mediator) : base(mediator)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var registered = bool.Parse(context.HttpContext.User.FindFirstValue("Registered"));

                if (!registered)
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
    }
    }

