using System;
using System.Collections.Generic;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class RegisterProviderOperationalAddressCommand : IRequest<CommandResponse<Provider>>
    {
		public RegisterProviderOperationalAddressCommand()
		{
		}
        public long ProviderId { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string County { get; set; }
        public double AreaLocation { get; set; }
        public List<int> ProductIds { get; set; }
    }
}

