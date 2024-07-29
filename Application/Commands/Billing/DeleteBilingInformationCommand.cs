using System;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application.Commands.Billing
{
	public class DeleteBilingInformationCommand : IRequest<CommandResponse<object>>
    {
		public long Id { get; set; }
		public long CompanyId { get; set; }
	}
}

