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
    public class AdminActivatelInvoiceHandler : IRequestHandler<AdminActivateInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;

        public AdminActivatelInvoiceHandler(IinvoiceRepository invoiceRepository, IBaseServiceAppointmentRepository _baseServiceAppointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._baseServiceAppointmentRepository = _baseServiceAppointmentRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminActivateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId, query=>query.Include(x=>x.BaseServiceAppointment))).FirstOrDefault();

            if (entity.Status != InvoiceStatus.Canceled && entity.Status != InvoiceStatus.Deleted)
            {
                return new NoAcess<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity), Message = "Your service is currently being processed." };
            }

            entity.Status = InvoiceStatus.Draft;
            await _invoiceRepository.UpdateAsync(entity);

            foreach (var item in entity.BaseServiceAppointment)
            {
                item.Status = ServiceStatus.Draft;
                await _baseServiceAppointmentRepository.UpdateAsync(item);
            }

            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity) };

        }

    }
}

