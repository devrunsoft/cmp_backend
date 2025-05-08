using System;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using CMPNatural.Core.Enums;


namespace CMPNatural.Application
{
	public class AdminCreateManifestHandler
	{
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IManifestRepository _repository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        public AdminCreateManifestHandler(IManifestRepository _repository, IinvoiceRepository _invoiceRepository, IAppInformationRepository _apprepository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository)
        {

            this._invoiceRepository = _invoiceRepository;
            this._apprepository = _apprepository;
            this._repository = _repository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
        }
        public async Task<CommandResponse<Manifest>> Create(Invoice invoice , ManifestStatus status)
		{
            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            var services = (await _invoiceRepository.GetAsync(x => x.Id == invoice.Id, query => query
                .Include(x => x.BaseServiceAppointment)
                .ThenInclude(x => x.Product)
                .Include(x => x.BaseServiceAppointment)
                .ThenInclude(x => x.ProductPrice)
                )).FirstOrDefault();

            var entity = new Manifest()
            {
                InvoiceId = invoice.Id,
                Status = status,
                Content = "",
                ContractId = invoice.ContractId.Value,
                CompanyId = invoice.CompanyId,
                CreatedAt = invoice.CreatedAt,
                ManifestNumber = ""
            };

            var result = await _repository.AddAsync(entity);
            await CreateManifestContent.CreateContent(services, information, entity, _serviceAppointmentLocationRepository);
            entity.ManifestNumber = entity.Number;
            await _repository.UpdateAsync(entity);
            return new Success<Manifest>() { Data = result, Message = "Successfull!" };

        }
    }
}

