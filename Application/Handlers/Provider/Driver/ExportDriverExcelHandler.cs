using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Model;
using CMPNatural.Application.Services;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class ExportDriverExcelHandler : IRequestHandler<ExportDriverExcelCommand, DriverExcelFileResult>
    {
        private readonly IProviderDriverRepository _providerDriverRepository;

        public ExportDriverExcelHandler(IProviderDriverRepository providerDriverRepository)
        {
            _providerDriverRepository = providerDriverRepository;
        }

        public async Task<DriverExcelFileResult> Handle(ExportDriverExcelCommand request, CancellationToken cancellationToken)
        {
            var drivers = (await _providerDriverRepository.GetAsync(
                x => x.ProviderId == request.ProviderId,
                query => query.Include(x => x.Driver).ThenInclude(x => x.Person)))
                .Select(x => new DriverResponse
                {
                    Id = x.Driver.Id,
                    PersonId = x.Driver.PersonId,
                    Email = x.Driver.Email,
                    Password = x.Driver.Password,
                    FirstName = x.Driver.Person?.FirstName ?? string.Empty,
                    LastName = x.Driver.Person?.LastName ?? string.Empty,
                    License = x.Driver.License ?? string.Empty,
                    LicenseExp = x.Driver.LicenseExp ?? default,
                    BackgroundCheck = x.Driver.BackgroundCheck ?? string.Empty,
                    BackgroundCheckExp = x.Driver.BackgroundCheckExp ?? default,
                    ProfilePhoto = x.Driver.ProfilePhoto,
                    ProviderId = x.ProviderId,
                    Status = x.Driver.Status,
                    TwoFactor = x.Driver.TwoFactor,
                    IsDefault = x.IsDefault
                })
                .ToList();

            return DriverExcelService.Export(drivers);
        }
    }
}
