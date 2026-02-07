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

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminSendMessageHandler : IRequestHandler<AdminSendMessageCommand, CommandResponse<ChatMessage>>
    {

        private readonly IChatMessageRepository _repository;
        private readonly IAdminRepository _adminRepository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        private readonly IFileStorage _fileStorage;
        public AdminSendMessageHandler(IChatMessageRepository _repository, IChatSessionRepository _chatSessionRepository
            , IChatService _chatService, IMediator _mediator, IAdminRepository _adminRepository, IFileStorage fileStorage)
        {
            this._adminRepository = _adminRepository;
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
            _fileStorage = fileStorage;
        }
        public async Task<CommandResponse<ChatMessage>> Handle(AdminSendMessageCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId , OperationalAddressId = request.OperationalAddressId})).Data;

            //var admin = await _adminRepository.GetOrCreateAsync(
            //    key: $"AdminEntity:id:{request.AdminId}",
            //    factory: async () =>
            //    {
            //        var list = await _adminRepository.GetAsync(x => x.Id == request.AdminId);
            //        return list.FirstOrDefault();
            //    }
            //);

            List<ChatMention> ChatMention = new List<ChatMention>();

            if (request.ChatMentionIds.Count > 0)
            {
                foreach (var item in request.ChatMentionIds)
                {
                    ChatMention.Add(
                        new ChatMention()
                        {
                            MentionedType= MentionedType.Admin,
                            MentionedId = item.Id,
                            OperationalAddressId = request.OperationalAddressId,
                            ClientId = request.ClientId,
                        }
                        );
                    var m = JsonConvert.SerializeObject(item, new StringEnumConverter());
                    await _chatService.SendToPerson(item.PersonId.ToString(), m, ChatEnum.mention);
                }
            }

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

            var entity = new ChatMessage
            {
                ChatSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = messageType,
                Content = request.Message ?? string.Empty,
                SenderId = request.AdminId,
                SenderType = SenderType.Admin,
                SentAt = DateTime.Now,
                OperationalAddressId = request.OperationalAddressId,
                ClientId = request.ClientId,
                Mentions = ChatMention,
                FileUrl = fileUrl,
                FileExtension = fileExtension,
                FileSize = fileSize
            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendMessageToClient(request.ClientId, entity);

            return new Success<ChatMessage>() { Data = result };
        }

    }
}
