using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Repositories.Chat;

namespace CMPNatural.Application
{
    public class AdminGetPaginateMessageManualNoteHandler : IRequestHandler<AdminGetPaginateMessageManualNoteCommand, CommandResponse<PagesQueryResponse<ChatMessageManualNote>>>
    {
        private readonly IChatMessageManualNoteRepository _repository;

        public AdminGetPaginateMessageManualNoteHandler(IChatMessageManualNoteRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatMessageManualNote>>> Handle(AdminGetPaginateMessageManualNoteCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => p.ChatSession.OperationalAddressId == request.OperationalAddressId,
                query => query.Include(x=>x.ChatSession)
                ));
            return new Success<PagesQueryResponse<ChatMessageManualNote>>() { Data = result };
        }
    }
}

