using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Mapper;
using Microsoft.Extensions.Options;
using CMPNatural.Core.Models;
using CMPNatural.Core.Extentions;

namespace CMPNatural.Application
{
    public class AdminGetManifestHandler : IRequestHandler<AdminGetManifestCommand, CommandResponse<ManifestResponse>>
    {
        private readonly IManifestRepository _repository;
        private readonly AppConfig _config;

        public AdminGetManifestHandler(IManifestRepository _repository, IOptions<AppConfig> config)
        {
            this._repository = _repository;
            _config = config.Value;
        }

        public async Task<CommandResponse<ManifestResponse>> Handle(AdminGetManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.Id == request.Id, query=> query

            .Include(x => x.Request)
            .ThenInclude(x=>x.OperationalAddress)
            .ThenInclude(x => x.LocationDateTimes)

            .Include(x => x.Request)
            .ThenInclude(x => x.BillingInformation)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.LocationCompany)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.Product)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ManifestGreaseServiceDetail)

            .Include(x => x.RouteServiceAppointmentLocation)
            .ThenInclude(x => x.Route)
            .ThenInclude(x => x.Driver)
            .ThenInclude(x => x.Person)

            .Include(x => x.Request)
            .ThenInclude(x=>x.Company)
            .Include(x => x.Provider)
            )).FirstOrDefault();

            var response = ManifestMapper.Mapper.Map<ManifestResponse>(result);
            response.OperationalAddressAddressId = _config.AddressId
                ? result.Request.OperationalAddress.Username
                : result.Request.OperationalAddress.Id.ToString();

            if (response.ServiceDateTime.HasValue)
            {
                var todayDayName = response.ServiceDateTime?.DayOfWeek.ToString();
                var openingHour = result.Request.OperationalAddress.LocationDateTimes
                    .FirstOrDefault(x => string.Equals(x.DayName, todayDayName, StringComparison.OrdinalIgnoreCase));

                if (openingHour != null)
                {
                    response.OpeningHours = $"{openingHour.FromTime.ConvertTimeToString()} until {openingHour.ToTime.ConvertTimeToString()}";
                }
            }

            return new Success<ManifestResponse>() { Data = response };
        }
    }
}
