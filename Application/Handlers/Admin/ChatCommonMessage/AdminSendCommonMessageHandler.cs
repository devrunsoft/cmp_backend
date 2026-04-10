using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories.Chat;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Hub;
using CMPNatural.Core.Repositories;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CMPFile;
using System.IO;
using CMPNatural;
using CMPNatural.Core.Extentions;
using CMPNatural.Core.Repositories.ChatCommon;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminSendCommonMessageHandler : IRequestHandler<AdminSendCommonMessageCommand, CommandResponse<ChatCommonMessage>>
    {

        private readonly IChatCommonMessageRepository _repository;
        private readonly IAdminRepository _adminRepository;
        private readonly IChatCommonSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        private readonly IFileStorage _fileStorage;
        public AdminSendCommonMessageHandler(IChatCommonMessageRepository _repository, IChatCommonSessionRepository _chatSessionRepository
            , IChatService _chatService, IMediator _mediator, IAdminRepository _adminRepository, IFileStorage fileStorage)
        {
            this._adminRepository = _adminRepository;
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
            _fileStorage = fileStorage;
        }
        public async Task<CommandResponse<ChatCommonMessage>> Handle(AdminSendCommonMessageCommand request, CancellationToken cancellationToken)
        {
            var admin = (await _adminRepository.GetOrCreateAsync(
               $"Admin:id:{request.AdminId}",
               async () => (await _adminRepository.GetAsync(x => x.Id == request.AdminId, query => query.Include(x => x.Person))).FirstOrDefault()
               ));

                var session = (await _chatSessionRepository.GetOrCreateAsync(
                $"Session:id:{request.ChatCommonSessionId}",
                 async () => (await _chatSessionRepository.GetAsync(x => x.Id == request.ChatCommonSessionId)).FirstOrDefault()
                 ));


            List<ChatMention> ChatMention = new List<ChatMention>();

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

            var entity = new ChatCommonMessage
            {
                PersonId= admin.PersonId,
                ChatCommonSessionId = request.ChatCommonSessionId,
                IsInternalNote = false,
                Type = messageType,
                Content = request.Message ?? string.Empty,
                SenderId = request.AdminId,
                SenderType = ParticipantType.Admin,
                SentAt = DateTime.Now,
                Mentions = ChatMention,
                FileUrl = fileUrl,
                FileExtension = fileExtension,
                FileSize = fileSize
            };
            var result = await _repository.AddAsync(entity);


            await _chatService.SendCommonMessageToPerson(session.PersonId.ToString(), entity);

            return new Success<ChatCommonMessage>() { Data = result };
        }

    }
}
