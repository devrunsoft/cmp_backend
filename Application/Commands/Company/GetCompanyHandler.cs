using System;
using CMPNatural.Application.Responses;
using MediatR;

namespace CMPNatural.Application.Commands.Company
{
	public class GetCompanyCommand : IRequest<CompanyResponse>
    {
		public long CompanyId { get; set; }
	}
}

