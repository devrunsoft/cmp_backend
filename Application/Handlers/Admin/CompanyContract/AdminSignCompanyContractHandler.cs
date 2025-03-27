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
        public AdminSignCompanyContractHandler(ICompanyContractRepository repository, IinvoiceRepository invoiceRepository, IAppInformationRepository apprepository,
            IManifestRepository _manifestRepository
            )
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _apprepository = apprepository;
            this._manifestRepository = _manifestRepository;
        }
        public async Task<CommandResponse<CompanyContract>> Handle(AdminSignCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.CompanyContractId);
            var appinformation = (await _apprepository.GetAllAsync()).FirstOrDefault();
            if(appinformation==null || appinformation.Sign == null)
            {
                return new NoAcess<CompanyContract>() { Message = "Please add a sign in Company Information" };
            }
            var content = entity.Content;
            content = CompanyContractHelper.ShowByKey(ContractKeysEnum.ManagmentCompanySign.GetDescription(), content, CompanyContractHelper.SignFont);
            content = content.Replace(ContractKeysEnum.ManagmentCompanySign.GetDescription(), appinformation.Sign);

            entity.Content = content;
            entity.Status = (int)CompanyContractStatus.Signed;
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
                await new AdminCreateManifestHandler(_manifestRepository, _invoiceRepository, _apprepository).Create(item);
            }

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

