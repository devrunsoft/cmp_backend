using System;
using System.Collections.Generic;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.ProviderServiceArea
{
	public class AdminAddProviderServiceAreaCommand : ServiceAreaInput, IRequest<CommandResponse<ServiceArea>>
    {
        public AdminAddProviderServiceAreaCommand(ServiceAreaInput input,long ProviderId)
        {
            this.ProviderId = ProviderId;
            City = input.City;
            State = input.State;
            ServiceAreaType = input.ServiceAreaType;
            Lat = input.Lat;
            Lng = input.Lng;
            Radius = input.Radius;
            GeoJson = input.GeoJson;
        }
        public long ProviderId { get; set; }
    }
}

