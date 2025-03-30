using System;
using CMPNatural.Application.Responses;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Company
{
	public class GetCompanyCommand : IRequest<CommandResponse<CompanyResponse>>
    {
		public long CompanyId { get; set; }
	}
}

