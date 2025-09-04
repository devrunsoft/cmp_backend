using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Entities.Base;
using System.Linq;
using CMPNatural.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminProcessingManifestHandler : IRequestHandler<AdminProcessingManifestCommand, CommandResponse<Manifest>>
    {
        private readonly IManifestRepository _repository;
        private readonly IinvoiceRepository _iinvoiceRepository;
        public AdminProcessingManifestHandler(IManifestRepository _repository, IinvoiceRepository _iinvoiceRepository)
        {
            this._repository = _repository;
            this._iinvoiceRepository = _iinvoiceRepository;
        }
        public async Task<CommandResponse<Manifest>> Handle(AdminProcessingManifestCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x=>x.Id == request.Id && x.Status == ManifestStatus.Scaduled,
                query=> query.Include(x=>x.Invoice).ThenInclude(x=>x.BaseServiceAppointment)
                )).FirstOrDefault();

            if (result == null)
            {
                return new NoAcess<Manifest>
                {
                    Message = "This manifest has already been assigned to a provider and cannot be reassigned."
                };
            }

            if (result.ProviderId == null)
            {
                return new NoAcess<Manifest>
                {
                    Message = "The manifest cannot be processed because no provider has been assigned. Please assign a provider first."
                };
            }

            //var invoices = await _iinvoiceRepository.GetAsync(x=>x.InvoiceId == result.Invoice.InvoiceId && (x.Status == InvoiceStatus.Processing_Provider));
            result.Status = ManifestStatus.Assigned;
            result.Invoice.Status = InvoiceStatus.Processing_Provider;
            foreach (var item in result.Invoice.BaseServiceAppointment)
            {
                item.Status = ServiceStatus.Proccessing;
            }


            await _repository.UpdateAsync(result);
            return new Success<Manifest>() { Data = result };
        }
    }
}

