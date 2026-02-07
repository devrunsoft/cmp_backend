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
using CMPFile;
using System.IO;
using CMPNatural.Core.Extentions;

namespace CMPNatural.Application
{
    public class AdminSendMessageManualNoteHandler : IRequestHandler<AdminSendMessageManualNoteCommand, CommandResponse<ChatMessageManualNote>>
    {
        private readonly IChatMessageManualNoteRepository _repository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        private readonly IFileStorage _fileStorage;
        public AdminSendMessageManualNoteHandler(IChatMessageManualNoteRepository _repository, IChatSessionRepository _chatSessionRepository , IChatService _chatService, IMediator _mediator, IFileStorage _fileStorage)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
            this._fileStorage = _fileStorage;
        }
        public async Task<CommandResponse<ChatMessageManualNote>> Handle(AdminSendMessageManualNoteCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId, OperationalAddressId = request.OperationalAddressId})).Data;

            var content = request.Message;

            string? fileUrl = null;
            string? fileExtension = null;
            long? fileSize = null;
            var messageType = MessageType.Message;

            if (request.File != null && request.File.Length > 0)
            {
                fileUrl = await _fileStorage.AppfileHandler(request.File);
                fileExtension = Path.GetExtension(request.File.FileName)?.TrimStart('.');
                fileSize = request.File.Length;
                messageType = MessageTypeResolver.ResolveMessageType(request.File.ContentType, fileExtension);
            }


            var entity = new ChatMessageManualNote
            {
                ChatSessionId = chatsession.Id,
                Content = content,
                SenderId = request.AdminId,
                SentAt = DateTime.Now,
                OperationalAddressId = chatsession.OperationalAddressId,
                ClientId = chatsession.ClientId,
                FileUrl = fileUrl,
                FileExtension = fileExtension,
                FileSize = fileSize,
                Type = messageType,
            };
            var result = await _repository.AddAsync(entity);

            return new Success<ChatMessageManualNote>() { Data = result };
        }
    }
}

