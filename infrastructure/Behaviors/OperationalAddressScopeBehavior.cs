using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application.Logger
{
    public class OperationalAddressScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OperationalAddressScopeBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst("OperationalAddressId")?.Value;
            if (long.TryParse(claimValue, out var operationalAddressId) && operationalAddressId > 0)
            {
                var property = request.GetType().GetProperty("OperationalAddressId", BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    if (property.PropertyType == typeof(long))
                    {
                        property.SetValue(request, operationalAddressId);
                    }
                    else if (property.PropertyType == typeof(long?))
                    {
                        property.SetValue(request, (long?)operationalAddressId);
                    }
                }
            }

            return await next();
        }
    }
}
