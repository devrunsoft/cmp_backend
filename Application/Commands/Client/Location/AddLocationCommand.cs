using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class AddLocationCompanyCommand : LocationCompanyInput, IRequest<CommandResponse<LocationCompany>>
    {
		public AddLocationCompanyCommand()
		{
		}

		public long CompanyId { get; set; }

	}
}

