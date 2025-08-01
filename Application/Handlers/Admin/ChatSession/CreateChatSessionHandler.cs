using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Repositories.Chat;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Api;
using ScoutDirect.Core.Repositories;
using Microsoft.AspNetCore.SignalR;
using CMPNatural.Application.Hub;

namespace CMPNatural.Application
{
    public class CreateChatSessionHandler : IRequestHandler<CreateChatSessionCommand, CommandResponse<ChatSession>>
    {
        private readonly IChatSessionRepository _repository;
        public CreateChatSessionHandler(IChatSessionRepository _repository)
        {
            this._repository = _repository;

        }
        public async Task<CommandResponse<ChatSession>> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _repository.GetAsync(x=>x.ClientId == request.ClientId && x.IsActive)).LastOrDefault();
            if (chatsession != null)
            {
                return new Success<ChatSession>() { Data = chatsession };
            }
            var entity = new ChatSession
            {
                ClientId = request.ClientId,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            var result = await _repository.AddAsync(entity);


            return new Success<ChatSession>() { Data = result };
        }
    }
}

