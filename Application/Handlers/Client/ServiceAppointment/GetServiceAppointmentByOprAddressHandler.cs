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
var currentStatuses = new[]
{
    ServiceStatus.Proccessing,

    ServiceStatus.Updated_Provider,
    ServiceStatus.Submited_Provider,
    ServiceStatus.In_Process,
    ServiceStatus.Arrived,
    ServiceStatus.Photo_Before_Work,
    ServiceStatus.Driver_Update_Service,
    ServiceStatus.Photo_After_Work,
    ServiceStatus.Done_Driver,
};
            var result = (await _serviceAppointmentRepository.GetAsync(
                (p)=> p.OperationalAddressId == request.OperationalAddressId &&
                p.CompanyId == request.CompanyId &&
                p.IsEmegency != true &&
                (
                p.Status == ServiceStatus.Draft
                ||
                  currentStatuses.Contains(p.Status) 
                ||
                p.Status == ServiceStatus.Scaduled) &&
                //
                p.Status != ServiceStatus.Canceled, query=> query
                .Include(x=>x.Request)
                .Include(x=>x.ProductPrice)
                )
                ).ToList();

            var data = result.GroupBy(x => x.ProductPrice.Id);

            var dataset = new List<ClientServiceAppointment>();

            foreach (var x in data)
            {
                var RequestId = x.FirstOrDefault().Request.Id;
                TerminateStatusEnum status = TerminateStatusEnum.None;
                status = await requestTerminateStatus(TerminateStatusEnum.None, RequestId, x);

                dataset.Add(new ClientServiceAppointment
                {
                    Draft = x.FirstOrDefault(p => p.Status == ServiceStatus.Draft),
                    Current = x.FirstOrDefault(p => currentStatuses.Contains(p.Status)),
                    Next = x.FirstOrDefault(p => p.Status == ServiceStatus.Scaduled),
                    ServiceId = x.FirstOrDefault(p => p.ProductPriceId == x.Key)?.ProductId ?? 0,
                    RequestId = RequestId,
                    TerminateStatus = status
                });
            }

            return new Success<List<ClientServiceAppointment>>() { Data = dataset };

        }
        public async Task<TerminateStatusEnum> requestTerminateStatus(TerminateStatusEnum status, long requestId, IGrouping<long, BaseServiceAppointment> x)
        {
            if(x.Any(p => p.Status == ServiceStatus.Scaduled))
            {
                status = TerminateStatusEnum.CanTerminate;
            }

            var terminateRequest = (await _requestTerminateRepository
                 .GetAsync(t => t.RequestId == requestId)).LastOrDefault();
        
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

