using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System;
using System.Linq;

namespace CMPNatural.Application
{
    public class AddInvoiceSourceHandler : IRequestHandler<AddInvoiceSourceCommand, CommandResponse<InvoiceSource>>
    {
        private readonly IInvoiceSourceRepository _repository;
        public AddInvoiceSourceHandler(IInvoiceSourceRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<InvoiceSource>> Handle(AddInvoiceSourceCommand request, CancellationToken cancellationToken)
        {

            var any = (await _repository.GetAsync(x => x.InvoiceId == request.InvoiceId)).Any();
            if (any)
            {
                return new NoAcess<InvoiceSource>() { Message = "Something went wrong!" };
            }
            var entity = new InvoiceSource
            {
                CreatedAt= DateTime.Now,
                CompanyId = request.CompanyId,
                InvoiceId = request.InvoiceId
            };

            var result = await _repository.AddAsync(entity);
            return new Success<InvoiceSource>() { Data = result };
        }
    }
}

