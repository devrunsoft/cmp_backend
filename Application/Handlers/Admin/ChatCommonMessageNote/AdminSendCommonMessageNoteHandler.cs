using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories.Chat;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Hub;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories.ChatCommon;
using System.Linq;
using System.Text.Json;

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminSendCommonMessageNoteHandler : IRequestHandler<AdminSendCommonMessageNoteCommand, CommandResponse<ChatCommonMessageNote>>
    {
        private readonly IChatCommonMessageNoteRepository _repository;
        private readonly IChatCommonSessionRepository _chatSessionRepository;
        private readonly IChatService _chatService;
        private readonly IMediator _mediator;
        public AdminSendCommonMessageNoteHandler(IChatCommonMessageNoteRepository _repository, IChatCommonSessionRepository _chatSessionRepository , IChatService _chatService, IMediator _mediator)
        {
            this._repository = _repository;
            this._chatSessionRepository = _chatSessionRepository;
            this._chatService = _chatService;
            this._mediator = _mediator;
        }
        public async Task<CommandResponse<ChatCommonMessageNote>> Handle(AdminSendCommonMessageNoteCommand request, CancellationToken cancellationToken)
        {
            var session = await _chatSessionRepository.GetOrCreateAsync(
                $"ChatCommonSession:id:{request.ChatCommonSessionId}",
                async () => (await _chatSessionRepository.GetAsync(x => x.Id == request.ChatCommonSessionId)).FirstOrDefault());

            var content = $"<b>{request.Type.Description()}</b><br>{request.Content}";

            var entity = new ChatCommonMessageNote
            {
                ChatCommonSessionId = request.ChatCommonSessionId,
                IsInternalNote = false,
                Type = MessageType.Note,
                MessageNoteType = request.Type,
                Content = content,
                SenderId = request.AdminId,
                SenderType = ParticipantType.Admin,
                SentAt = DateTime.Now,
                PersonId = request.PersonId,
                Payload = request.Data == null ? null : JsonSerializer.Serialize(request.Data)
            };
            var result = await _repository.AddAsync(entity);

            await _chatService.SendCommonMessageToPerson(session.PersonId.ToString(), entity);
            await _chatService.SendCommonToAllAdmins(entity);

            return new Success<ChatCommonMessageNote>() { Data = result };
        }
    }
}
