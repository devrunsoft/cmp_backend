using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{

    public class GetAllInvoicePayableHandler : IRequestHandler<GetAllInvoicePayableCommand, CommandResponse<List<Invoice>>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public GetAllInvoicePayableHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<List<Invoice>>> Handle(GetAllInvoicePayableCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.CompanyId == request.CompanyId&& p.Status == (int)InvoiceStatus.SentForPay)).ToList();

            return new Success<List<Invoice>>() { Data = entity, Message = "Successfull!" };

        }

    }
}

