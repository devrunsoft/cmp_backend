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

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminSendMessageNoteHandler : IRequestHandler<AdminSendMessageNoteCommand, CommandResponse<ChatMessageNote>>
    {
        private readonly IChatMessageNoteRepository _repository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public AdminSendMessageNoteHandler(IChatMessageNoteRepository _repository, IChatSessionRepository _chatSessionRepository , IChatService _chatService, IMediator _mediator)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatMessageNote>> Handle(AdminSendMessageNoteCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId})).Data;

            var content = $"<b>{request.Type.Description()}</b><br>{request.Content}";

            var entity = new ChatMessageNote
            {
                ChatSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = MessageType.Note,
                MessageNoteType = request.Type,
                Content = content,
                SenderId = request.AdminId,
                SenderType = SenderType.Admin,
                SentAt = DateTime.Now
            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendMessageToClient(request.ClientId, entity);
            await _chatService.SendToAllAdmins(entity);

            return new Success<ChatMessageNote>() { Data = result };
        }
    }
}

