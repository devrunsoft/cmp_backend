using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using System.Linq;

namespace CMPNatural.Application.Handlers
{

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, CommandResponse<Invoice>>
    {
        private readonly IinvoiceRepository _invoiceRepository;

        public CreateInvoiceHandler(IinvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<Invoice>> Handle(CreateInvoiceCommand requests, CancellationToken cancellationToken)
        {
            List<BaseServiceAppointment> lstCustom = new List<BaseServiceAppointment>();
            List<ServiceAppointmentEmergency> lstCustomEmrgency = new List<ServiceAppointmentEmergency>();

            foreach (var request in requests.Services)
            {
                if(request.ServiceKind == Core.Enums.ServiceKind.Custom)
                {
                 var command = new ServiceAppointment()
                    {
                        CompanyId = requests.CompanyId,
                        FrequencyType = request.FrequencyType,
                        //LocationCompanyId=request.LocationCompanyId,
                        ServiceTypeId = (int)request.ServiceTypeId,
                        ServicePriceCrmId = request.ServicePriceId,
                        ServiceCrmId = request.ServiceCrmId,
                        StartDate = request.StartDate??DateTime.Now,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = (int)ServiceStatus.draft,
                        IsEmegency=false,
                        Qty = request.qty,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                        .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                        .ToList()
                 };
                    lstCustom.Add(command);
                }
                else
                {
                    var command = new ServiceAppointmentEmergency()
                    {
                        CompanyId = requests.CompanyId,
                        FrequencyType = request.FrequencyType,
                        //LocationCompanyId=request.LocationCompanyId,
                        ServiceTypeId = (int)request.ServiceTypeId,
                        ServicePriceCrmId = request.ServicePriceId,
                        ServiceCrmId = request.ServiceCrmId,
                        //StartDate = request.StartDate,
                        OperationalAddressId = request.OperationalAddressId,
                        Status = (int)ServiceStatus.draft,
                        IsEmegency = true,
                        Qty = request.qty,
                        ServiceAppointmentLocations = request.LocationCompanyIds
                        .Select(id => new ServiceAppointmentLocation { LocationCompanyId = id })
                        .ToList()
                    };
                    lstCustom.Add(command);
                }

            }


            var entity = new Invoice()
            {
                CompanyId = requests.CompanyId,
                InvoiceCrmId= requests.InvoiceCrmId,
                Status = "draft",
                InvoiceId = requests.InvoiceId,
                BaseServiceAppointment = lstCustom,
                Amount = requests.Amount,
                RegisterDate= DateTime.Now
                //InvoiceNumber = request.InvoiceNumber
            };

            var result = await _invoiceRepository.AddAsync(entity);

            return new Success<Invoice>() { Data = result, Message = "Successfull!" };

        }

    }
}

