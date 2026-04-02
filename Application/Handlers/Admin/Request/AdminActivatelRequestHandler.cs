using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Mapper;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Commands.Admin.Request;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application.Handlers
{
    public class AdminActivatelRequestHandler : IRequestHandler<AdminActivateRequestCommand, CommandResponse<RequestEntity>>
    {
        private readonly IRequestRepository _invoiceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;

        public AdminActivatelRequestHandler(IRequestRepository invoiceRepository, IBaseServiceAppointmentRepository _baseServiceAppointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._baseServiceAppointmentRepository = _baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<RequestEntity>> Handle(AdminActivateRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.RequestId, query => query
            .Include(x => x.BaseServiceAppointment).ThenInclude(x => x.ServiceAppointmentLocations)
            )).FirstOrDefault();

            if (entity.Status != InvoiceStatus.Canceled && entity.Status != InvoiceStatus.Deleted)
            {
                return new NoAcess<RequestEntity>() { Data = entity, Message = "Your service is currently being processed." };
            }

            entity.Status = InvoiceStatus.Draft;
            await _invoiceRepository.UpdateAsync(entity);

            foreach (var i in entity.BaseServiceAppointment)
            {
                foreach (var item in i.ServiceAppointmentLocations)
                {
                    item.Status = ServiceStatus.Draft;
                    await _baseServiceAppointmentRepository.UpdateAsync(i);
                }
            }

            return new Success<RequestEntity>() { Data = entity };

        }

    }
}

