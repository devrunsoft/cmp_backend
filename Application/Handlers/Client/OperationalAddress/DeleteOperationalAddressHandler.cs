using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;

using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using CMPNatural.Application.Commands.OperationalAddress;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Net;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class DeleteOperationalAddressHandler : IRequestHandler<DeleteOperationalAddressCommand, CommandResponse<OperationalAddress>>
    {
        private readonly IOperationalAddressRepository _operationalAddressRepository;
        private readonly ILocationCompanyRepository _locationCompanyRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;

        public DeleteOperationalAddressHandler(IOperationalAddressRepository operationalAddressRepository,
            IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, ILocationCompanyRepository _locationCompanyRepository)
        {
            _operationalAddressRepository = operationalAddressRepository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
            this._locationCompanyRepository = _locationCompanyRepository;
        }

        public async Task<CommandResponse<OperationalAddress>> Handle(DeleteOperationalAddressCommand request, CancellationToken cancellationToken)
        {

            var entity = (await _operationalAddressRepository.GetAsync(p=>p.Id== request.Id && p.CompanyId == request.CompanyId,
                query=> query.Include(x=>x.LocationCompany))).FirstOrDefault();

            if (entity == null)
            {
                return new NoAcess<OperationalAddress>() { Message = "No access to delete!" };
            }

            if (entity.LocationCompany.Count() > 0)
            {
                foreach (var item in entity.LocationCompany)
                {
                   if ((await _serviceAppointmentLocationRepository.GetAsync(x => x.LocationCompanyId == item.Id)).Any())
                   {
                        return new NoAcess<OperationalAddress>() { Message = "This location cannot be deleted because it is associated with one or more service appointments." };
                   }
                }
            }
            await _locationCompanyRepository.DeleteRangeAsync(entity.LocationCompany.ToList());
            await _operationalAddressRepository.DeleteAsync(entity);

            return new Success<OperationalAddress>() { Success = true, Data = entity, Message = "OperationalAddres deleted successfully!" };
        }

    }
}
