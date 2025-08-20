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
using CMPNatural.Application.Commands;

namespace CMPNatural.Application.Handlers.Admin
{
    public class ClientSendMessageNoteHandler : IRequestHandler<ClientSendMessageNoteCommand, CommandResponse<ChatMessageNote>>
    {
        private readonly IChatMessageNoteRepository _repository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public ClientSendMessageNoteHandler(IChatMessageNoteRepository _repository, IChatService _chatService, IMediator _mediator)
        {
            this._repository = _repository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatMessageNote>> Handle(ClientSendMessageNoteCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatSessionCommand() { ClientId = request.ClientId, OperationalAddressId = request.OperationalAddressId})).Data;

            var content = $"<b>{request.Type.Description()}</b><br>{request.Content}";

            var entity = new ChatMessageNote
            {
                ChatSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = MessageType.Note,
                MessageNoteType = request.Type,
                Content = content,
                SenderId = request.ClientId,
                SenderType = SenderType.Client,
                SentAt = DateTime.Now,
                ClientId = request.ClientId,
                OperationalAddressId = request.OperationalAddressId
            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendToAllAdmins(entity);
            await _chatService.SendMessageToClient(request.ClientId, entity);

            return new Success<ChatMessageNote>() { Data = result };
        }
    }
}

