using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using ScoutDirect.Application.Responses;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Enums;

public class MenuAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly int _menuId;

    public MenuAuthorizeAttribute(MenuEnum menuId)
    {
        _menuId = (int)menuId;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        var isSuperAdmin = user.FindFirst(ClaimTypes.Role)?.Value;
        if (isSuperAdmin == "SuperAdmin")
        {
            return;
        }

        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new ForbidResult(); // User is not authenticated
            return;
        }

        var adminId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(adminId))
        {
            context.Result = new ForbidResult();
            return;
        }

        // Get the service to check menu access
        var mediator = context.HttpContext.RequestServices.GetService<IMediator>();

        if (mediator == null)
        {
            context.Result = new StatusCodeResult(500); // Internal server error if service is missing
            return;
        }
        // Check access
        try
        {
            var hasAccess = await mediator.Send(new CheckMenuAccessQuery(int.Parse(adminId), _menuId));
            if (!hasAccess.IsSucces())
            {
                context.Result = new ForbidResult();
            }
        }
        catch (Exception ex)
        {
            context.Result = new ForbidResult();
        }


    }
}
