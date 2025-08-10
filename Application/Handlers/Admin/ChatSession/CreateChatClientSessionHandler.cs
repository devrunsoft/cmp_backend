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
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ScoutDirect.Core.Entities.Base;

namespace CMPNatural.Application
{
    public class CreateChatClientSessionHandler : IRequestHandler<CreateChatClientSessionCommand, CommandResponse<ChatClientSession>>
    {
        private readonly IChatClientSessionRepository _repository;
        public CreateChatClientSessionHandler(IChatClientSessionRepository _repository)
        {
            this._repository = _repository;

        }
        public async Task<CommandResponse<ChatClientSession>> Handle(CreateChatClientSessionCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _repository.GetAsync(x => x.ClientId == request.ClientId && x.IsActive,
                query => query.Include(x => x.ChatSession)
                )).LastOrDefault();

            if (chatsession != null)
            {
                List<ChatSession> chatSessions = new List<ChatSession>();
                chatSessions.AddRange(chatsession.ChatSession);
                foreach (var item in request.OperationalAddressId)
                {
                    if (!chatSessions.Any(x => x.Id == item))
                    {
                        chatSessions.Add(new ChatSession()
                        {
                            ClientId = request.ClientId,
                            CreatedAt = DateTime.Now,
                            IsActive = true,
                            OperationalAddressId = item
                        });
                    }
                }
                chatsession.ChatSession = chatSessions;
                 await _repository.UpdateAsync(chatsession);

                return new Success<ChatClientSession>() { Data = chatsession };
            }
            else
            {
                var entity = new ChatClientSession
                {
                    ClientId = request.ClientId,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    ChatSession = request.OperationalAddressId.Select(x => new ChatSession()
                    {
                        ClientId = request.ClientId,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        OperationalAddressId = x
                    }).ToList()
                };
                var result = await _repository.AddAsync(entity);
                return new Success<ChatClientSession>() { Data = result };
            }


        }
    }
}

