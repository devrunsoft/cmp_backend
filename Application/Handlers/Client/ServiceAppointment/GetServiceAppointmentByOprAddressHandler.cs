using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Responses.ClientServiceAppointment;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace CMPNatural.Application.Handlers
{

    public class GetServiceAppointmentByOprAddressHandler : IRequestHandler<GetServiceAppointmentByOprAddressCommand, CommandResponse<List<ClientServiceAppointment>>>
    {
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IRequestTerminateRepository _requestTerminateRepository;

        public GetServiceAppointmentByOprAddressHandler(IBaseServiceAppointmentRepository billingInformationRepository, IRequestTerminateRepository _requestTerminateRepository)
        {
            _serviceAppointmentRepository = billingInformationRepository;
            this._requestTerminateRepository = _requestTerminateRepository;
        }

        public async Task<CommandResponse<List<ClientServiceAppointment>>> Handle(GetServiceAppointmentByOprAddressCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetAsync(
                (p)=> p.OperationalAddressId == request.OperationalAddressId &&
                p.CompanyId == request.CompanyId &&
                p.IsEmegency != true &&
                (
                p.Status == ServiceStatus.Draft
                ||
                p.Status == ServiceStatus.Proccessing
                ||
                p.Status == ServiceStatus.Scaduled) &&
                //
                p.Status != ServiceStatus.Canceled, query=> query
                .Include(x=>x.Invoice)
                .Include(x=>x.ProductPrice)
                )
                ).ToList();

            var data = result.GroupBy(x => x.ProductPrice.Id);

            var dataset = new List<ClientServiceAppointment>();

            foreach (var x in data)
            {
                var invoiceNumber = x.FirstOrDefault().Invoice.InvoiceId;
                TerminateStatusEnum status = TerminateStatusEnum.None;
                status = await requestTerminateStatus(TerminateStatusEnum.None, invoiceNumber, x);

                dataset.Add(new ClientServiceAppointment
                {
                    Draft = x.FirstOrDefault(p => p.Status == ServiceStatus.Draft),
                    Current = x.FirstOrDefault(p => p.Status == ServiceStatus.Proccessing),
                    Next = x.FirstOrDefault(p => p.Status == ServiceStatus.Scaduled),
                    ServiceId = x.FirstOrDefault(p => p.ProductPriceId == x.Key)?.ProductId ?? 0,
                    InvoiceNumber = invoiceNumber,
                    TerminateStatus = status
                });
            }

            return new Success<List<ClientServiceAppointment>>() { Data = dataset };

        }
        public async Task<TerminateStatusEnum> requestTerminateStatus(TerminateStatusEnum status, string invoiceNumber, IGrouping<long, BaseServiceAppointment> x)
        {
            if(x.Any(p => p.Status == ServiceStatus.Scaduled))
            {
                status = TerminateStatusEnum.CanTerminate;
            }

            var terminateRequest = (await _requestTerminateRepository
                 .GetAsync(t => t.InvoiceNumber == invoiceNumber)).LastOrDefault();
        
            if (terminateRequest != null)
            {
                if (terminateRequest.RequestTerminateStatus == null)
                {
                    status = TerminateStatusEnum.Requested;
                    return status;
                }
                else if (terminateRequest.RequestTerminateStatus == RequestTerminateProcessEnum.Updated)
                {
                    //status = TerminateStatusEnum.Updated;
                    return status;
                }
            }
            return status;
        }

    }
}

