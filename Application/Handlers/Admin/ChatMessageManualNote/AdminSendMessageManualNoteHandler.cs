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
using CMPNatural.Application.Commands;

namespace CMPNatural.Application
{
    public class AdminSendMessageManualNoteHandler : IRequestHandler<AdminSendMessageManualNoteCommand, CommandResponse<ChatMessageManualNote>>
    {
        private readonly IChatMessageManualNoteRepository _repository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public AdminSendMessageManualNoteHandler(IChatMessageManualNoteRepository _repository, IChatSessionRepository _chatSessionRepository , IChatService _chatService, IMediator _mediator)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatMessageManualNote>> Handle(AdminSendMessageManualNoteCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId, OperationalAddressId = request.OperationalAddressId})).Data;

            var content = request.Message;

            var entity = new ChatMessageManualNote
            {
                ChatSessionId = chatsession.Id,
                Content = content,
                SenderId = request.AdminId,
                SentAt = DateTime.Now,
                OperationalAddressId = chatsession.OperationalAddressId,
                ClientId = chatsession.ClientId
            };
            var result = await _repository.AddAsync(entity);

            return new Success<ChatMessageManualNote>() { Data = result };
        }
    }
}

