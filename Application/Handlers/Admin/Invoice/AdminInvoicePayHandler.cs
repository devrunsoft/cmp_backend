using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Invoice;
using ScoutDirect.Core.Entities.Base;
using System.Linq;
using CMPNatural.Core.Enums;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using System.ServiceModel.Channels;

namespace CMPNatural.Application.Handlers
{
    public class AdminInvoicePayHandler : IRequestHandler<AdminInvoicePayCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;

        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        public AdminInvoicePayHandler(IinvoiceRepository invoiceRepository,IBaseServiceAppointmentRepository _baseServiceAppointmentRepository,
             IManifestRepository _manifestRepository)
        {
            _invoiceRepository = invoiceRepository;
            this._baseServiceAppointmentRepository = _baseServiceAppointmentRepository;
            this._manifestRepository = _manifestRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminInvoicePayCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _invoiceRepository.GetAsync(p => p.Id == request.InvoiceId, query =>
            query
            .Include(x => x.BaseServiceAppointment).ThenInclude(x => x.ServiceAppointmentLocations)
            )).FirstOrDefault();

            if(entity.Status != InvoiceStatus.Send_Payment)
            {
                return new NoAcess<InvoiceResponse>() {
                    Message = $"Invoice {InvoiceMapper.Mapper.Map<InvoiceResponse>(entity).InvoiceNumber} cannot be marked as paid because its current status is '{entity.Status.GetDescription()}." };
            }
            entity.Status = InvoiceStatus.Complete;
            entity.PaymentStatus = PaymentStatus.Manual_Paid;



            foreach (var i in entity.BaseServiceAppointment)
            {
                foreach (var serviceAppointment in i.ServiceAppointmentLocations)
                {
                    serviceAppointment.Status = ServiceStatus.Complete;
                    await _baseServiceAppointmentRepository.UpdateAsync(i);
                }
            }
            await _invoiceRepository.UpdateAsync(entity);

            var manifest = (await _manifestRepository.GetAsync(x => x.RequestId == request.InvoiceId)).FirstOrDefault();
            manifest.Status = ManifestStatus.Canceled;
            await _manifestRepository.UpdateAsync(manifest);

            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(entity) };

        }
    }
}

