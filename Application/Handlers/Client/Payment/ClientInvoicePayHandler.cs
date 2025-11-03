using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Application.Mapper;
using CMPNatural.Core.Enums;
using CMPPayment;
using CMPNatural.Core.Entities;
using System;

namespace CMPNatural.Application
{
    public class ClientInvoicePayHandler : IRequestHandler<ClientInvoicePayCommand, CommandResponse<string>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IPaymentConfiguration _paymentConfiguration;
        private readonly IPaymentRepository _paymentRepository;

        public ClientInvoicePayHandler(IinvoiceRepository invoiceRepository, IPaymentConfiguration _paymentConfiguration, IPaymentRepository _paymentRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._paymentConfiguration = _paymentConfiguration;
            this._paymentRepository = _paymentRepository;
        }

        public async Task<CommandResponse<string>> Handle(ClientInvoicePayCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId,
                query => query
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i=>i.ProductPrice)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                )).FirstOrDefault();

            var model = InvoiceMapper.Mapper.Map<InvoiceResponse>(invoices);

            if (invoices.Status != InvoiceStatus.Send_Payment)
            {
                return new NoAcess<string>() { Message = $"Invoice with ID {model.InvoiceNumber} was not found." };
            }
            var data = _paymentConfiguration.CreatePayment(invoices.BaseServiceAppointment.ToList());
            await _paymentRepository.AddAsync(new Payment() {
                Amount = invoices.Amount ,
                CheckoutSessionId = data.Id,
                CreateAt = DateTime.Now,
                CompanyId = invoices.CompanyId,
                InvoiceId = invoices.Id,
                Status = PaymentHistoryStatus.Draft,
                Content = data.ToString()
            });

            return new Success<string>() { Data = data.Url };
        }
    }
}

