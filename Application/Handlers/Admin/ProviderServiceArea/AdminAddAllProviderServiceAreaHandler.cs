using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Commands.Admin.ProviderServiceArea;
using System.Collections.Generic;
using CMPNatural.Core.Repositories;

namespace CMPNatural.Application
{

    public class AdminAddAllProviderServiceAreaHandler : IRequestHandler<AdminAddAllProviderServiceAreaCommand, CommandResponse<List<ServiceArea>>>
    {
        private readonly IServiceAreaRepository _repository;
        public AdminAddAllProviderServiceAreaHandler(IServiceAreaRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<List<ServiceArea>>> Handle(AdminAddAllProviderServiceAreaCommand request, CancellationToken cancellationToken)
        {
            List<ServiceArea> lst = new List<ServiceArea>();

            foreach (var i in request)
            {
                lst.Add(new ServiceArea
                {
                    Active = true,
                    City = i.City,
                    CreateAt = DateTime.Now,
                    GeoJson = i.GeoJson,
                    Lat = i.Lat,
                    Lng = i.Lng,
                    ProviderId = request.ProviderId,
                    Radius = i.Radius,
                    ServiceAreaType = i.ServiceAreaType,
                    State = i.State,
                    Address = i.Address
                });
            }


             await _repository.AddRangeAsync(lst);
            return new Success<List<ServiceArea>>() { Data = lst };
        }
    }

}