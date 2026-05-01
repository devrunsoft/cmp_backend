using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.Application.Commands;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class AdminGetAllManualNoteHandler : IRequestHandler<AdminGetAllManualNoteCommand, CommandResponse<PagesQueryResponse<ChatMessageManualNote>>>
    {
        private readonly IChatMessageManualNoteRepository _repository;

        public AdminGetAllManualNoteHandler(IChatMessageManualNoteRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatMessageManualNote>>> Handle(AdminGetAllManualNoteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, null,
                query => query.Include(x => x.ChatSession)
                ));
            return new Success<PagesQueryResponse<ChatMessageManualNote>>() { Data = result };
        }
    }
}

