using System;
using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
	public class AdminUpdateRequestCommand : InvoiceInput, IRequest<CommandResponse<RequestResponse>>
    {
		public AdminUpdateRequestCommand()
		{
		}

		public long RequestId { get; set; }
	}
}

