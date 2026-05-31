using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Core.Services
{
    public interface IHostVerificationService
    {
        string CreateSignature(string challenge);
        Task<bool> VerifyHostAsync(string? host, CancellationToken cancellationToken = default);
    }
}
