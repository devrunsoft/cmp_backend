using Bazaro.Application.Commands;
using Bazaro.Application.Mapper;
using Bazaro.Application.Queries;
using Bazaro.Application.Responses;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Responses.Invoice;
using Bazaro.Core.Entities;
using Bazaro.Core.Enums;
using Bazaro.Core.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CommandResponse>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInboxRepository _inboxRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IInboxStatusRepository _inboxStatusRepository;
        private readonly IMediator _mediator;

        public CreateInvoiceHandler(IInvoiceRepository invoiceRepository, IChatRepository chatRepository,
            IInboxRepository inboxRepository, IInboxStatusRepository inboxStatusRepository, IMediator mediator)
        {
            _invoiceRepository = invoiceRepository;
            _inboxRepository = inboxRepository;
            _chatRepository = chatRepository;
            _inboxStatusRepository = inboxStatusRepository;
            _mediator = mediator;
        }
        public async Task<CommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            //var inbox = await _mediator.Send(new GetInboxInfoQuery() { Id = request.InboxId });
            var inbox = await _inboxRepository.GetByIdAsync(request.InboxId);

            var _now = DateTime.Now;
            var newModel = new Invoice()
            {
                DiscountId = request.DiscountId,
                Price = request.Price,
                // CahtId = Convert.ToInt64(chat.Id),
                CustomerId = inbox.CustomerId,
                ShopUserId = request.ShopUserId,
                AddressId = inbox.AddressId,
                DeliveryId = request.DeliveryId,

                IsActive = true,
                CreatedAt = _now,

                Caht = new Chat
                {
                    InboxId = request.InboxId,
                    TypeId = (int)ChatType.Invoice,
                    Body = "فاکتور ارسال شد",
                    ShopUserId = request.ShopUserId,
                    IsActive = true,
                    CreatedAt = _now,
                }
            };

            var invoice = await _invoiceRepository.AddAsync(newModel);

            //var resultModel = await _invoiceRepository.AddInvoiceAsync(request.InboxId,newModel);

            inbox.LastChatId = invoice.CahtId;
            inbox.InboxStatusId = (int)ShopInboxStatus.InvoiceIssued;
            await _inboxRepository.UpdateAsync(inbox);

            var chat = await _chatRepository.GetChatByIdAsync(invoice.CahtId);
            var inboxStatus = await _inboxStatusRepository.GetByIdAsync((int)ShopInboxStatus.InvoiceIssued);

            return new Success()
            {
                Data = new CreateInvoiceResponse()
                {
                    ChatData = ChatMapper.Mapper.Map<ChatResponse>(chat),
                    InboxStatusData = InboxStatusMapper.Mapper.Map<InboxStatusResponse>(inboxStatus)
                }
            };
        }
    }
}
