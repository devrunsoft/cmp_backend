using System.Threading;
using CMPNatural.Core.Models;

namespace CMPNatural.Core.Services
{
    public static class TenantExecutionContext
    {
        private static readonly AsyncLocal<TenantRequestContext?> CurrentTenant = new();

        public static TenantRequestContext? Current
        {
            get => CurrentTenant.Value;
            set => CurrentTenant.Value = value;
        }
    }
}
