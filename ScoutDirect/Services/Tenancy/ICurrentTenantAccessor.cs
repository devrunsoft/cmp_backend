using CMPNatural.Core.Models;

namespace CMPNatural.Api.Services
{
    public interface ICurrentTenantAccessor
    {
        TenantRequestContext? Current { get; set; }
    }
}
