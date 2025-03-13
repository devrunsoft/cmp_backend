using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminDeleteProductPriceCommand : IRequest<CommandResponse<ProductPrice>>
    {
		public AdminDeleteProductPriceCommand()
		{
		}

		public long Id { get; set; }
	}
}

