using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminDeleteProductCommand : IRequest<CommandResponse<Product>>
    {
		public AdminDeleteProductCommand()
		{
		}

        public long Id { get; set; }
    }
}

