using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Commands
{
	public class AdminGetAllManualNoteCommand: PagedQueryRequest, IRequest<CommandResponse<PagesQueryResponse<ChatMessageManualNote>>>
    {
		public AdminGetAllManualNoteCommand()
		{
		}
	}
}

