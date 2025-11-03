using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands.Admin.Request;

namespace CMPNatural.Application.Handlers.Admin.Invoice
{
    public class AdminGetRequestHandler : IRequestHandler<AdminGetRequestCommand, CommandResponse<RequestResponse>>
    {
        private readonly IRequestRepository _invoiceRepository;

        public AdminGetRequestHandler(IRequestRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<RequestResponse>> Handle(AdminGetRequestCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetAsync(p => p.Id == request.RequestId,
                query => query.Include(i => i.Company)
                .ThenInclude(x => x.BillingInformations)
                .Include(i => i.Provider)
                .Include(x => x.BaseServiceAppointment)
                .ThenInclude(i => i.ProductPrice)
                .ThenInclude(p => p.Product)
              .Include(x => x.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                .ThenInclude(p => p.LocationCompany)
                .Include(i => i.BillingInformation)
                )).FirstOrDefault();

            var model = RequestMapper.Mapper.Map<RequestResponse>(invoices);

            return new Success<RequestResponse>() { Data = model };
        }
    }
}

