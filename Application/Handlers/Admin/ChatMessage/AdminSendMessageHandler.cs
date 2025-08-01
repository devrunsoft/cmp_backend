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

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminSendMessageHandler : IRequestHandler<AdminSendMessageCommand, CommandResponse<ChatMessage>>
    {

        private readonly IChatMessageRepository _repository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public AdminSendMessageHandler(IChatMessageRepository _repository, IChatSessionRepository _chatSessionRepository , IChatService _chatService, IMediator _mediator)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatMessage>> Handle(AdminSendMessageCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId})).Data;

            var entity = new ChatMessage
            {
                ChatSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = MessageType.Message,
                Content = request.Message,
                SenderId = request.AdminId,
                SenderType = SenderType.Admin,
                SentAt = DateTime.Now
            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendMessageToClient(request.ClientId, entity);

            return new Success<ChatMessage>() { Data = result };
        }
    }
}

