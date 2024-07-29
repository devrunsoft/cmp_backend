using Bazaro.Application.Commands;
using Bazaro.Application.Mapper;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Validator;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class ShopSetInboxStatusHandler : IRequestHandler<ShopSetInboxStatusCommand, CommandResponse>
    {
        private readonly IInboxStatusRepository _inboxStatusRepository;
        private readonly IInboxRepository _inboxRepository;

        public ShopSetInboxStatusHandler(IInboxStatusRepository inboxStatusRepository, IInboxRepository inboxRepository)
        {
            _inboxStatusRepository = inboxStatusRepository;
            _inboxRepository = inboxRepository;
        }

        public async Task<CommandResponse> Handle(ShopSetInboxStatusCommand request, CancellationToken cancellationToken)
        {
            var inbox = await _inboxRepository.GetByIdAsync(request.InboxId);

            if (inbox == null)
                return new ResponseNotFound();

            var results = new ShopSetInboxStatusCommandValidator(inbox).Validate(request);

            if (!results.IsValid)
                return new HasError(results);

            inbox.InboxStatusId = request.StatusId;

            inbox.UpdatedAt = DateTime.Now;
            await _inboxRepository.UpdateAsync(inbox);

            var inboxStatus = await _inboxStatusRepository.GetByIdAsync(request.StatusId);
            var inboxStatusResponse = InboxStatusMapper.Mapper.Map<InboxStatusResponse>(inboxStatus);

            return new Success() { Data = inboxStatusResponse };
        }
    }
}
