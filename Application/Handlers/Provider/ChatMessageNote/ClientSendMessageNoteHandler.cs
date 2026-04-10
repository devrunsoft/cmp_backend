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
    public class ParticipantSendMessageNoteHandler : IRequestHandler<ParticipantSendCommonMessageNoteCommand, CommandResponse<ChatCommonMessageNote>>
    {
        private readonly IChatCommonMessageNoteRepository _repository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public ParticipantSendMessageNoteHandler(IChatCommonMessageNoteRepository _repository, IChatService _chatService, IMediator _mediator)
        {
            this._repository = _repository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatCommonMessageNote>> Handle(ParticipantSendCommonMessageNoteCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _mediator.Send(new CreateChatCommonSessionCommand()
            {
                ParticipantId = request.ParticipantId,
                ParticipantType = request.ParticipantType
            })).Data;

            var content = $"<b>{request.Type.Description()}</b><br>{request.Content}";

            var entity = new ChatCommonMessageNote
            {
                ChatCommonSessionId = chatsession.Id,
                IsInternalNote = false,
                Type = MessageType.Note,
                MessageNoteType = request.Type,
                Content = content,
                SenderId = request.ParticipantId,
                SenderType = request.ParticipantType,
                SentAt = DateTime.Now,
                PersonId = request.PersonId

            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendCommonToAllAdmins(entity);
            await _chatService.SendCommonMessageToPerson(request.PersonId.ToString(), entity);

            return new Success<ChatCommonMessageNote>() { Data = result };
        }
    }
}
