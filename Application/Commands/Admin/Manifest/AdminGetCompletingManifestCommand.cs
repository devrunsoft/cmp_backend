using System;
using System.Collections.Generic;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminGetCompletingManifestCommand : IRequest<CommandResponse<List<Manifest>>>
    {
		public AdminGetCompletingManifestCommand()
		{
		}
		public long CompanyId { get; set; }
        public long OperationalAddressId { get; set; }
    }
}

