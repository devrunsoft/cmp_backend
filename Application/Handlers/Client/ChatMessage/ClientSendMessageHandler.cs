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
using CMPNatural.Application.Commands.Client;
using CMPFile;
using System.IO;
using CMPNatural;

namespace CMPNatural.Application.Handlers.Admin
{
    public class ClientSendMessageHandler : IRequestHandler<ClientSendMessageCommand, CommandResponse<ChatMessage>>
    {
        private readonly IChatMessageRepository _repository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        private readonly IFileStorage _fileStorage;
        public ClientSendMessageHandler(IChatMessageRepository _repository, IChatSessionRepository _chatSessionRepository, IChatService _chatService, IMediator _mediator, IFileStorage fileStorage)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
            _fileStorage = fileStorage;
        }
        public async Task<CommandResponse<ChatMessage>> Handle(ClientSendMessageCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId, OperationalAddressId = request.OperationalAddressId })).Data;

            string? fileUrl = null;
            string? fileExtension = null;
            long? fileSize = null;
            var messageType = MessageType.Message;

            if (request.File != null && request.File.Length > 0)
            {
                fileUrl = await _fileStorage.AppfileHandler(request.File);
                fileExtension = Path.GetExtension(request.File.FileName)?.TrimStart('.');
                fileSize = request.File.Length;
                messageType = ResolveMessageType(request.File.ContentType, fileExtension);
            }

            var entity = new ChatMessage
            {
                ChatSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = messageType,
                Content = request.Message ?? string.Empty,
                SenderId = request.ClientId,
                SenderType = SenderType.Client,
                SentAt = DateTime.Now,
                OperationalAddressId = request.OperationalAddressId,
                ClientId = request.ClientId,
                FileUrl = fileUrl,
                FileExtension = fileExtension,
                FileSize = fileSize
            };
            var result = await _repository.AddAsync(entity);


            await _chatService.SendToAllAdmins(entity);

            return new Success<ChatMessage>() { Data = result };
        }

        private static MessageType ResolveMessageType(string? contentType, string? extension)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                if (contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    return MessageType.IMAGE;
                if (contentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                    return MessageType.VIDEO;
                if (contentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
                    return MessageType.AUDIO;
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                var ext = extension.ToLowerInvariant();
                if (ext is "jpg" or "jpeg" or "png" or "gif" or "bmp" or "webp")
                    return MessageType.IMAGE;
                if (ext is "mp4" or "mov" or "avi" or "mkv" or "webm")
                    return MessageType.VIDEO;
                if (ext is "mp3" or "wav" or "aac" or "ogg" or "flac")
                    return MessageType.AUDIO;
            }

            return MessageType.FILE;
        }
    }
}
