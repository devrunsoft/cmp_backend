using System.Security.Claims;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using CMPNatural.Core.Repositories;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Api.Services
{
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ITenantRepository tenantRepository,
            ICurrentTenantAccessor currentTenantAccessor,
            AppSetting appSetting)
        {
            
            var host = context.Request.Host.Host?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(host))
            {
                await _next(context);
                return;
            }
            //var TenantId = long.TryParse(context.User.FindFirstValue("TenantId"), out var claimTenantId);

            var tenantContext = await ResolveTenantContextAsync(host, tenantRepository, appSetting);
            if (tenantContext != null)
            {
                tenantContext.PortalType = ResolvePortalType(context.Request.Path);
                currentTenantAccessor.Current = tenantContext;
            }

            try
            {

                //var expectedPortal = ResolveExpectedPortal(context.Request.Path);
                //if (expectedPortal.HasValue && tenantContext?.PortalType.HasValue == true && tenantContext.PortalType != expectedPortal)
                //{
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    await context.Response.WriteAsJsonAsync(new
                //    {
                //        Success = false,
                //        Message = "This domain is not allowed to access the requested portal."
                //    });
                //    return;
                //}

                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var claimValue = context.User.FindFirstValue("TenantId");
                    if (long.TryParse(claimValue, out var claimTenantId) && claimTenantId != tenantContext?.TenantId)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Success = false,
                            Message = "This session is not valid for the requested tenant domain. Please sign in again."
                        });
                        return;
                    }
                }

                await _next(context);
            }
            finally
            {
                currentTenantAccessor.Current = null;
            }
        }

        private static async Task<TenantRequestContext?> ResolveTenantContextAsync(
            string host,
            ITenantRepository tenantRepository,
            AppSetting appSetting)
        {
            var tenant = await tenantRepository.GetActiveByHostAsync(host);
            if (tenant != null)
            {
                return await CreateTenantContextAsync(tenant, tenantRepository);
            }

            if (!MatchesHost(appSetting.adminHost, host) &&
                !MatchesHost(appSetting.clientHost, host) &&
                !MatchesHost(appSetting.providerHost, host))
            {
                return null;
            }

            return new TenantRequestContext
            {
                Host = host,
                IsResolvedFromDatabase = false
            };
        }

        private static async Task<TenantRequestContext> CreateTenantContextAsync(Tenant tenant, ITenantRepository tenantRepository)
        {
            var accessList = await tenantRepository.GetActiveAccessListAsync(tenant.Id);
            var accessibleTenantIds = accessList
                .Where(x => x.CanViewRecords)
                .Select(x => x.AccessibleTenantId)
                .Distinct()
                .ToList();

            return new TenantRequestContext
            {
                TenantId = tenant.Id,
                TenantName = tenant.Name,
                Host = tenant.Host ?? string.Empty,
                CanViewAllRecords = tenant.AdminCanViewAllRecords,
                CanManageDispatch = tenant.WantsDispatchManagement &&
                                    accessList.Any(x => x.CanManageDispatch),
                AccessibleTenantIds = accessibleTenantIds,
                IsResolvedFromDatabase = true
            };
        }

        private static bool MatchesHost(string? configuredUrl, string requestHost)
        {
            if (string.IsNullOrWhiteSpace(configuredUrl))
            {
                return false;
            }

            return Uri.TryCreate(configuredUrl, UriKind.Absolute, out var uri)
                ? string.Equals(uri.Host, requestHost, StringComparison.OrdinalIgnoreCase)
                : string.Equals(configuredUrl.Trim().ToLowerInvariant(), requestHost, StringComparison.OrdinalIgnoreCase);
        }

        private static PortalType? ResolvePortalType(PathString path)
        {
            if (path.StartsWithSegments("/api/provider", StringComparison.OrdinalIgnoreCase))
            {
                return PortalType.Provider;
            }

            if (path.StartsWithSegments("/api/driver", StringComparison.OrdinalIgnoreCase))
            {
                return PortalType.Driver;
            }

            if (path.StartsWithSegments("/api/admin", StringComparison.OrdinalIgnoreCase))
            {
                return PortalType.Admin;
            }

            if (path.StartsWithSegments("/api/client", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWithSegments("/api/user", StringComparison.OrdinalIgnoreCase))
            {
                return PortalType.Client;
            }

            return null;
        }
    }
}
