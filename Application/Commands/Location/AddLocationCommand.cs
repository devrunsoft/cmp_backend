using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands
{
	public class AddLocationCompanyCommand : LocationCompanyInput, IRequest<CommandResponse<object>>
    {
		public AddLocationCompanyCommand()
		{
		}

		public long CompanyId { get; set; }

	}
}

