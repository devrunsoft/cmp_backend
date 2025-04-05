using System;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.ServiceAppointment;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.ServiceAppointmentEmergency;

namespace CMPNatural.Application.Handlers
{

    public class DeleteServiceAppointmentEmergencyHandler : IRequestHandler<DeleteServiceAppointmentEmergencyCommand, CommandResponse<object>>
    {
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IinvoiceRepository _iinvoiceRepository;
        private readonly ICompanyContractRepository _companyContractRepository;

        public DeleteServiceAppointmentEmergencyHandler(IBaseServiceAppointmentRepository _serviceAppointmentRepository, IManifestRepository _manifestRepository,
            IinvoiceRepository _iinvoiceRepository, ICompanyContractRepository _companyContractRepository)
        {
            this._serviceAppointmentRepository = _serviceAppointmentRepository;
            this._manifestRepository = _manifestRepository;
            this._iinvoiceRepository = _iinvoiceRepository;
            this._companyContractRepository = _companyContractRepository;
        }


        public async Task<CommandResponse<object>> Handle(DeleteServiceAppointmentEmergencyCommand request, CancellationToken cancellationToken)
        {
            var result = (await _serviceAppointmentRepository.GetAsync(p=>p.Id== request.Id && p.CompanyId==request.CompanyId)).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<object>() { Message = "No Access To Cancel This Service" };
            }

            result = await new CancelService(_serviceAppointmentRepository, _manifestRepository, _iinvoiceRepository, _companyContractRepository).cancel(result, CancelEnum.ByClient);
            return new Success<object>() { Data = result, Message = "Service Deleted Successfully!" };

        }

    }
}

