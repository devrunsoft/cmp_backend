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

    public class AdminAddProviderServiceAreaHandler : IRequestHandler<AdminAddProviderServiceAreaCommand, CommandResponse<ServiceArea>>
    {
        private readonly IServiceAreaRepository _repository;
        public AdminAddProviderServiceAreaHandler(IServiceAreaRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<ServiceArea>> Handle(AdminAddProviderServiceAreaCommand i, CancellationToken cancellationToken)
        {
            //List<ServiceArea> lst = new List<ServiceArea>();

            //foreach (var i in request) {
            //    lst.Add(new ServiceArea
            //    {
            //        Active = i.Active,
            //        City = i.City,
            //        CreateAt=DateTime.Now,
            //        GeoJson = i.GeoJson,
            //        Lat = i.Lat,
            //        Lng=i.Lng,
            //        ProviderId = request.ProviderId,
            //        Radius = i.Radius,
            //        ServiceAreaType=i.ServiceAreaType,
            //        State = i.State
            //    });
            //}
            var entity = new ServiceArea
            {
                Active = true,
                City = i.City,
                CreateAt = DateTime.Now,
                GeoJson = i.GeoJson,
                Lat = i.Lat,
                Lng = i.Lng,
                ProviderId = i.ProviderId,
                Radius = i.Radius,
                ServiceAreaType = i.ServiceAreaType,
                State = i.State
            };

            var result = await _repository.AddAsync(entity);
            return new Success<ServiceArea>() { Data = result };
        }
    }

}