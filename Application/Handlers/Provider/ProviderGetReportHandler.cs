using System;
using CMPNatural.Application.Commands.Invoice;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Responses.Report;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Provider;

namespace CMPNatural.Application.Handlers
{

    public class ProviderGetReportHandler : IRequestHandler<ProviderGetReportCommand, CommandResponse<ReportResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public ProviderGetReportHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<ReportResponse>> Handle(ProviderGetReportCommand request, CancellationToken cancellationToken)
        {
            if(request.ProviderId == 0)
            {
                return new NoAcess<ReportResponse>() {};
            }
            var entity = (await _invoiceRepository.GetAsync(x=> x.ProviderId == request.ProviderId));
            DateTime lastMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime lastMonthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

              var AllInvoiceAmount=  entity.Sum(x => x.Amount);
            var roundedAllInvoiceAmount = AllInvoiceAmount.ToString("#,0.00");

            var CompleteInvoiceAmount = entity.Where(x => x.Status == InvoiceStatus.Complete).Sum(x => x.Amount);
            var roundedCompleteInvoiceAmount = CompleteInvoiceAmount.ToString("#,0.00");

            var ActiveInvoiceAmount = entity.Where(x => x.Status != InvoiceStatus.Complete).Sum(x => x.Amount);
            var roundedActiveInvoiceAmount = ActiveInvoiceAmount.ToString("#,0.00");

            var data = new ReportResponse()
            {
                AllInvoiceAmount = roundedAllInvoiceAmount,
                CompleteInvoiceAmount = roundedCompleteInvoiceAmount,
                ActiveInvoiceAmount = roundedActiveInvoiceAmount,
                //
                ThisMountInvoiceAmount = new MounthReportEntity() {
                    Amount = Math.Round(entity
                .Where(x => x.CreatedAt.Year == DateTime.Now.Year && x.CreatedAt.Month == DateTime.Now.Month)
                .Sum(x => x.Amount), 3) ,
                  Date = DateTime.Now,
                   CurrentMount = true
                } ,
                //
                LastMountInvoiceAmount = new MounthReportEntity() {
                    Amount =
                     Math.Round(entity
                .Where(x => x.CreatedAt >= lastMonthStart && x.CreatedAt <= lastMonthEnd)
                .Sum(x => x.Amount), 3),
                  Date = lastMonthStart,
                   CurrentMount = false
                } 
              };

            return new Success<ReportResponse>() { Data = data };

        }

    }
}

