using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMPNatural.Api.Attribute
{
    public class GlobalSuperAdminAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }

            var tenantIdClaim = user.FindFirst("TenantId")?.Value;
            if (!string.IsNullOrWhiteSpace(tenantIdClaim))
            {
                context.Result = new JsonResult(new
                {
                    Success = false,
                    Message = "This session is tenant-scoped and cannot access global super admin APIs. Please sign in with a global super admin account."
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            return Task.CompletedTask;
        }
    }
}
