using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories.Chat;

namespace CMPNatural.Application
{
    public class ClientGetPaginateMessageHandler : IRequestHandler<ClientGetPaginateMessageCommand, CommandResponse<PagesQueryResponse<ChatMessage>>>
    {
        private readonly IChatMessageRepository _repository;

        public ClientGetPaginateMessageHandler(IChatMessageRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<ChatMessage>>> Handle(ClientGetPaginateMessageCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, p => p.ChatSession.ClientId == request.CompanyId && p.ChatSession.OperationalAddressId == request.OperationalAddressId,
                query => query.Include(x=>x.ChatSession)
                ));
            return new Success<PagesQueryResponse<ChatMessage>>() { Data = result };
        }
    }
}

