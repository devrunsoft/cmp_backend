using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;
using CMPNatural.Core.Enums;
using System.Linq;
using CMPNatural.Core.Helper;

namespace CMPNatural.Application
{
    public class AdminSignCompanyContractHandler : IRequestHandler<AdminSignCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly ICompanyContractRepository _repository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        public AdminSignCompanyContractHandler(ICompanyContractRepository repository, IinvoiceRepository invoiceRepository, IAppInformationRepository apprepository,
            IManifestRepository _manifestRepository, IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository
            )
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _apprepository = apprepository;
            this._manifestRepository = _manifestRepository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
        }
        public async Task<CommandResponse<CompanyContract>> Handle(AdminSignCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.CompanyContractId);
            var appinformation = (await _apprepository.GetAllAsync()).FirstOrDefault();
            if(appinformation==null || appinformation.Sign == null)
            {
                return new NoAcess<CompanyContract>() { Message = "Please add a sign in Company Information" };
            }

            if (entity.Status != CompanyContractStatus.Needs_Admin_Signature)
            {
                return new NoAcess<CompanyContract>
                {
                    Message = "Contract cannot be signed at this stage. It must be in 'Needs Admin Signature' status."
                };
            }

            var content = entity.Content;
            content = CompanyContractHelper.ShowByKey(ContractKeysEnum.ManagmentCompanySign.GetDescription(), content, CompanyContractHelper.SignFont);
            content = content.Replace(ContractKeysEnum.ManagmentCompanySign.GetDescription(), appinformation.Sign);

            entity.Content = content;
            entity.Status = CompanyContractStatus.Signed;
            entity.AdminSign = appinformation.Sign;
            await _repository.UpdateAsync(entity);

            //update Invoice
            var invoice = await _invoiceRepository.GetAsync(x => x.InvoiceId == entity.InvoiceId && x.Status == InvoiceStatus.Needs_Admin_Signature &&
            x.CompanyId == request.CompanyId
            );

            foreach (var item in invoice)
            {
                item.Status = InvoiceStatus.Needs_Assignment;
                await _invoiceRepository.UpdateAsync(item);
                await new AdminCreateManifestHandler(_manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository).Create(item, ManifestStatus.Draft);
                await CreateScaduleServiceHandler.Create(item, _manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository);
            }

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

