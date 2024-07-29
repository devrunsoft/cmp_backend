using Bazaro.Application.Commands;
using Bazaro.Application.Mapper;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using Bazaro.Core.Entities;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CreateInboxHandler : IRequestHandler<CreateOrOpenInboxCommand, CommandResponse>
    {
        private readonly IInboxRepository _inboxRepository;

        public CreateInboxHandler(IInboxRepository inboxRepository)
        {
            _inboxRepository = inboxRepository;
        }

        public async Task<CommandResponse> Handle(CreateOrOpenInboxCommand request, CancellationToken cancellationToken)
        {
            var inbox = await _inboxRepository.GetInbox(request.ShopId, request.CustomerId, request.AddressId);

            if (inbox != null)
            {
                inbox.UpdatedAt = System.DateTime.Now;
                inbox.InboxStatusId = (int)(Core.Enums.CustomerInboxStatus.NewOrder);
                inbox.AddressId = request.AddressId;

                await _inboxRepository.UpdateAsync(inbox); 
            }
            else
            {
                inbox = CreateInboxMapper.Mapper.Map<Inbox>(request);

                inbox.IsActive = true;
                inbox.CreatedAt = System.DateTime.Now;
                inbox.UpdatedAt = inbox.CreatedAt;
                inbox.InboxStatusId = (int)(Core.Enums.CustomerInboxStatus.NewOrder);

                await _inboxRepository.AddAsync(inbox);
            }

            inbox = await _inboxRepository.GetInbox(request.ShopId, request.CustomerId, request.AddressId);
            return new Success() { Id = inbox.Id, Data = InboxCustomerMapper.Mapper.Map<InboxResponse>(inbox) };
        }
    }
}
