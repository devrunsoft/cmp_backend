using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Validator;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class ShopDeleteInboxHandler : IRequestHandler<ShopDeleteInboxCommand, CommandResponse>
    {
        private readonly IInboxRepository _inboxRepository;

        public ShopDeleteInboxHandler(IInboxRepository inboxRepository)
        {
            _inboxRepository = inboxRepository;
        }

        public async Task<CommandResponse> Handle(ShopDeleteInboxCommand request, CancellationToken cancellationToken)
        {
            var inbox = await _inboxRepository.GetByIdAsync(request.InboxId);

            if (inbox == null)
                return new ResponseNotFound();
 
            var results = new ShopDeleteInboxCommandValidator(inbox).Validate(request);

            if (!results.IsValid)
                return new HasError(results);
 
            inbox.IsActive = false;
            await _inboxRepository.UpdateAsync(inbox);

            return new Success() { };
        }
    }
}
