using CMPNatural.Core.Models;
using CMPNatural.Core.Services;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Api.Services
{
    public class CurrentTenantAccessor : ICurrentTenantAccessor
    {
        private const string TenantContextKey = "__tenant_context";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TenantRequestContext? Current
        {
            get
            {
                var httpContextValue = _httpContextAccessor.HttpContext?.Items[TenantContextKey] as TenantRequestContext;
                return httpContextValue ?? TenantExecutionContext.Current;
            }
            set
            {
                var httpContext = _httpContextAccessor.HttpContext;
                TenantExecutionContext.Current = value;

                if (httpContext == null)
                {
                    return;
                }

                httpContext.Items[TenantContextKey] = value;
            }
        }
    }
}
