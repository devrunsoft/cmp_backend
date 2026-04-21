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
using CMPNatural.Application.Services;
using CMPFile;
using System.IO;
using CMPNatural;
using CMPNatural.Core.Repositories.ChatCommon;

namespace CMPNatural.Application.Handlers.Admin
{
    public class ParticipantSendCommonMessageHandler : IRequestHandler<ParticipantSendCommonMessageCommand, CommandResponse<ChatCommonMessage>>
    {
        private readonly IChatCommonMessageRepository _repository;
        private readonly IChatCommonSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        private readonly IFileStorage _fileStorage;
        private readonly IUnseenMessageEmailScheduler _unseenMessageEmailScheduler;
        public ParticipantSendCommonMessageHandler(IChatCommonMessageRepository _repository, IChatCommonSessionRepository _chatSessionRepository, IChatService _chatService,
            IMediator _mediator, IFileStorage fileStorage, IUnseenMessageEmailScheduler unseenMessageEmailScheduler)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
            _fileStorage = fileStorage;
            _unseenMessageEmailScheduler = unseenMessageEmailScheduler;
        }
        public async Task<CommandResponse<ChatCommonMessage>> Handle(ParticipantSendCommonMessageCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatCommonSessionCommand() {
                ParticipantId = request.ParticipantId,
                ParticipantType = request.ParticipantType
            })).Data;

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

            var entity = new ChatCommonMessage
            {
                ChatCommonSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = messageType,
                Content = request.Message ?? string.Empty,
                SenderId = request.ParticipantId,
                SenderType = request.ParticipantType,
                SentAt = DateTime.Now,
                FileUrl = fileUrl,
                FileExtension = fileExtension,
                FileSize = fileSize,
                PersonId = chatsession.PersonId
            };
            var result = await _repository.AddAsync(entity);
            _unseenMessageEmailScheduler.ScheduleCommonMessageEmail(result.Id);


            await _chatService.SendCommonToAllAdmins(entity);

            return new Success<ChatCommonMessage>() { Data = result };
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
