using System;
using CMPNatural.Api.Controllers.Admin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMPNatural.Api.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]/{providerId}")]
    public class BaseAdminProviderApiController : BaseAdminApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long _id;

        public long rProviderId
        {
            get
            {
                return _id;
            }
        }

        public BaseAdminProviderApiController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
            : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            _id = GetIdFromRoute()!.Value;
        }

        private int? GetIdFromRoute()
        {
            var routeValues = _httpContextAccessor.HttpContext?.Request.RouteValues;
            if (routeValues != null && routeValues.TryGetValue("providerId", out var idValue))
            {
                if (int.TryParse(idValue?.ToString(), out int id))
                    return id;
            }
            return null;
        }
    }
}

