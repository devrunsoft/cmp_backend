using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Admin.ProviderServiceArea
{
	public class AdminDeleteProviderServiceAreaCommand: IRequest<CommandResponse<ServiceArea>>
    {
		public AdminDeleteProviderServiceAreaCommand(long Id,long ProviderId)
		{
			this.Id = Id;
			this.ProviderId = ProviderId;
        }
		public long Id { get; set; }
        public long ProviderId { get; set; }
    }
}

