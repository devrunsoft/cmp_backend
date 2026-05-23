using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Model;
using CMPNatural.Application.Services;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminExportProviderExcelHandler : IRequestHandler<AdminExportProviderExcelCommand, ProviderExcelFileResult>
    {
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminExportProviderExcelHandler(IProviderReposiotry providerReposiotry)
        {
            _providerReposiotry = providerReposiotry;
        }

        public async Task<ProviderExcelFileResult> Handle(AdminExportProviderExcelCommand request, CancellationToken cancellationToken)
        {
            var providers = (await _providerReposiotry.GetAsync(
                x => true,
                query => query.Include(x => x.ProviderService).ThenInclude(x => x.Product)))
                .OrderBy(x => x.Id)
                .ToList();

            return ProviderExcelService.Export(providers);
        }
    }
}
