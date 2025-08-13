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

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminSendMessageHandler : IRequestHandler<AdminSendMessageCommand, CommandResponse<ChatMessage>>
    {

        private readonly IChatMessageRepository _repository;
        private readonly IAdminRepository _adminRepository;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public AdminSendMessageHandler(IChatMessageRepository _repository, IChatSessionRepository _chatSessionRepository
            , IChatService _chatService, IMediator _mediator, IAdminRepository _adminRepository)
        {
            this._adminRepository = _adminRepository;
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatMessage>> Handle(AdminSendMessageCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId , OperationalAddressId = request.OperationalAddressId})).Data;

            var admin = await _adminRepository.GetOrCreateAsync(
                key: $"AdminEntity:id:{request.AdminId}",
                factory: async () =>
                {
                    var list = await _adminRepository.GetAsync(x => x.Id == request.AdminId);
                    return list.FirstOrDefault();
                }
            );

            var entity = new ChatMessage
            {
                ChatSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = MessageType.Message,
                Content = request.Message,
                SenderId = request.AdminId,
                SenderType = SenderType.Admin,
                SentAt = DateTime.Now,
                OperationalAddressId = request.OperationalAddressId,
                ClientId = request.ClientId
            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendMessageToClient(request.ClientId, entity);
            //await _chatService.AdminUserTyping(admin.PersonId.ToString(), new UserTypingPayload()
            //{ ClientId = request.ClientId, IsTyping = false  , OperationalAddressId = request.OperationalAddressId, Name =  }
            //);

            return new Success<ChatMessage>() { Data = result };
        }
    }
}

