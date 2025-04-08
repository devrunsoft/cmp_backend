using System;
using System.Collections.Generic;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.ProviderServiceArea
{
	public class AdminAddAllProviderServiceAreaCommand : List<ServiceAreaInput>, IRequest<CommandResponse<List<ServiceArea>>>
    {
        public AdminAddAllProviderServiceAreaCommand(List<ServiceAreaInput> input,long ProviderId):base(input)
        {
            this.ProviderId = ProviderId;

        }
        public long ProviderId { get; set; }
    }
}

