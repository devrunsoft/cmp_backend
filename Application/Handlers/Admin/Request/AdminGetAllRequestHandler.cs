using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;

using ScoutDirect.Core.Authentication;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminGetAllRequestHandler : IRequestHandler<AdminGetAllRequestCommand, CommandResponse<PagesQueryResponse<RequestResponse>>>
    {
        private readonly IRequestRepository _invoiceRepository;

        public AdminGetAllRequestHandler(IRequestRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<RequestResponse>>> Handle(AdminGetAllRequestCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetBasePagedAsync(request, x => (request.Status == null || x.Status == request.Status),
                query => query.Include(i => i.Company)));

            var model = new PagesQueryResponse<RequestResponse>(
                invoices.elements.Select(p => RequestMapper.Mapper.Map<RequestResponse>(p)).ToList(),
                invoices.pageNumber,
                invoices.totalPages,
                invoices.totalElements);

            return new Success<PagesQueryResponse<RequestResponse>>() { Data = model };
        }
    }
}

