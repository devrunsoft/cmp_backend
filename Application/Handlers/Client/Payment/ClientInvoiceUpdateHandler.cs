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
using CMPNatural.Application.Commands.Client.Payment;

namespace CMPNatural.Application
{
    public class ClientInvoiceUpdateHandler : IRequestHandler<ClientInvoicePaidCommand, CommandResponse<bool>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IPaymentConfiguration _paymentConfiguration;
        private readonly IPaymentRepository _paymentRepository;

        public ClientInvoiceUpdateHandler(IinvoiceRepository invoiceRepository, IPaymentConfiguration _paymentConfiguration, IPaymentRepository _paymentRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._paymentConfiguration = _paymentConfiguration;
            this._paymentRepository = _paymentRepository;
        }

        public async Task<CommandResponse<bool>> Handle(ClientInvoicePaidCommand request, CancellationToken cancellationToken)
        {

            var paymentStrip = await _paymentConfiguration.GetPayment(request.CheckoutSessionId);

            var payment = (await _paymentRepository.GetAsync(x=>x.CheckoutSessionId == request.CheckoutSessionId)).FirstOrDefault();

            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == payment.InvoiceId)).FirstOrDefault();


            if (paymentStrip.PaymentStatus == "paid")
            {
                payment.Status = PaymentHistoryStatus.Paid;
                invoice.PaymentStatus = PaymentStatus.Paid;
                invoice.Status = InvoiceStatus.Complete;
                await _invoiceRepository.UpdateAsync(invoice);

            }
            else
            {
                payment.Status = PaymentHistoryStatus.Canceled;
            }
            payment.Content = paymentStrip.ToString();
            await _paymentRepository.UpdateAsync(payment);

            return new Success<bool>() { Data = paymentStrip.PaymentStatus == "paid" };
        }
    }
}

