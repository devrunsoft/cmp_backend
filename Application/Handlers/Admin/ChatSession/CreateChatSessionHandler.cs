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
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class CreateChatSessionHandler : IRequestHandler<CreateChatSessionCommand, CommandResponse<ChatSession>>
    {
        private readonly IChatSessionRepository _repository;
        private readonly IMediator _mediator;
        public CreateChatSessionHandler(IChatSessionRepository _repository, IMediator _mediator)
        {
            this._repository = _repository;
            this._mediator = _mediator;

        }
        public async Task<CommandResponse<ChatSession>> Handle(CreateChatSessionCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _repository.GetAsync(x=>x.OperationalAddressId == request.OperationalAddressId && x.IsActive)).LastOrDefault();
            if (chatsession != null)
            { 
                return new Success<ChatSession>() { Data = chatsession };
            }
            else
            {
                var result = (await _mediator.Send(new CreateChatClientSessionCommand() {
                    ClientId = request.ClientId, OperationalAddressId =  new List<long> { request.OperationalAddressId }
                }));

                if (result.IsSucces())
                {
                    return new Success<ChatSession>() { Data = result.Data.ChatSession.SingleOrDefault(x=>x.OperationalAddressId == request.OperationalAddressId) };
                }
            }

            return new NoAcess<ChatSession>() { };
        }
    }
}

