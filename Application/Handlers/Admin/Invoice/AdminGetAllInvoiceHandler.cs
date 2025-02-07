using System;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Repositories;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Core.Base;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminGetAllInvoiceHandler : IRequestHandler<AdminGetAllInvoiceCommand, CommandResponse<PagesQueryResponse<Invoice>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public AdminGetAllInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Invoice>>> Handle(AdminGetAllInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetBasePagedAsync(request, query => query.Include(i => i.Company)));

            return new Success<PagesQueryResponse<Invoice>>() { Data = invoices };

        }

    }
}

