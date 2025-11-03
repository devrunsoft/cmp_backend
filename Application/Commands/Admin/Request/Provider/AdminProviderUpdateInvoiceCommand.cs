using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminProviderUpdateRequestCommand : InvoiceInput, IRequest<CommandResponse<RequestResponse>>
    {
		public AdminProviderUpdateRequestCommand()
		{
		}

		public long RequestId { get; set; }
	}
}

