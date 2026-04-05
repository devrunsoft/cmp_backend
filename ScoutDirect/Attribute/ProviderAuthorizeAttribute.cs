using System;
using CMPNatural.Application.Commands.Admin;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMPNatural.Api.Attribute
{
	public class ProviderAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            var IsDriver = user.FindFirst("IsDriver")?.Value;
            if (IsDriver == "True")
            {
               context.Result = new ForbidResult();
            }
        }
    }
}

