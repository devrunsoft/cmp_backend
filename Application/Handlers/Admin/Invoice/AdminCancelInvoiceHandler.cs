using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Mapper;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers
{
    public class AdminCancelInvoiceHandler : IRequestHandler<AdminCancelInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;

        public AdminCancelInvoiceHandler(IinvoiceRepository invoiceRepository, IBaseServiceAppointmentRepository _baseServiceAppointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._baseServiceAppointmentRepository = _baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminCancelInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId, query => query
            .Include(x => x.BaseServiceAppointment).ThenInclude(x => x.ServiceAppointmentLocations)
            )).FirstOrDefault();

            if (entity.Status != InvoiceStatus.Draft)
            {
                return new NoAcess<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity), Message = "Your service is currently being processed." };
            }

            entity.Status = InvoiceStatus.Deleted;
            await _invoiceRepository.UpdateAsync(entity);

            foreach (var i in entity.BaseServiceAppointment)
            {
                foreach (var item in i.ServiceAppointmentLocations)
                {
                    item.Status = ServiceStatus.Canceled;
                    await _baseServiceAppointmentRepository.UpdateAsync(i);
                }
            }

            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity) };

        }

    }
}

