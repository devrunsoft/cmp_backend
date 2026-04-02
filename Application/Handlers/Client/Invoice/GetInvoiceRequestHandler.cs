using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands.Client.Invoice;

namespace CMPNatural.Application.Handlers
{

    public class GetInvoiceRequestHandler : IRequestHandler<GetInvoiceRequestCommand, CommandResponse<RequestResponse>>
    {
        private readonly IRequestRepository _invoiceRepository;
        public GetInvoiceRequestHandler(IRequestRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<RequestResponse>> Handle(GetInvoiceRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.CompanyId == request.CompanyId && p.Id == request.Id &&
            (request.OperationalAddressId == 0 || p.OperationalAddressId == request.OperationalAddressId)
            , query => query
            .Include(p => p.BaseServiceAppointment)
            )).FirstOrDefault();
            return new Success<RequestResponse>() { Data = RequestMapper.Mapper.Map<RequestResponse>(entity) };
        }
    }
}

