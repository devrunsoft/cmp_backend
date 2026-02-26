using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using System.Linq;
using CMPNatural.Core.Helper;
using CMPNatural.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class AdminSignCompanyContractHandler : IRequestHandler<AdminSignCompanyContractCommand, CommandResponse<CompanyContract>>
    {
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly ICompanyContractRepository _repository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IRequestRepository _invoiceRepository;
        private readonly IRequestRepository requestRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly AppSetting _appSetting;
        private readonly ILocationCompanyRepository locationCompanyRepository;

        public AdminSignCompanyContractHandler(ICompanyContractRepository repository, IRequestRepository invoiceRepository, IRequestRepository requestRepository,  IAppInformationRepository apprepository,
            IManifestRepository _manifestRepository, IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, AppSetting _appSetting,
            ILocationCompanyRepository locationCompanyRepository, IBaseServiceAppointmentRepository _baseServiceAppointmentRepository)
        {
            this._baseServiceAppointmentRepository = _baseServiceAppointmentRepository;
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            this.requestRepository = requestRepository;
            _apprepository = apprepository;
            this._manifestRepository = _manifestRepository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
            this._appSetting = _appSetting;
            this.locationCompanyRepository = locationCompanyRepository;
        }
        public async Task<CommandResponse<CompanyContract>> Handle(AdminSignCompanyContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.CompanyContractId);
            var appinformation = (await _apprepository.GetAllAsync()).LastOrDefault();
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
            var invoice = await requestRepository.GetAsync(x => x.Id == entity.RequestId && x.Status == InvoiceStatus.Needs_Admin_Signature &&
            x.CompanyId == request.CompanyId, query=> query
            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ServiceAppointmentLocations)

            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.BaseServiceAppointment)
            .ThenInclude(x => x.Product)
            );
            var invid = Guid.NewGuid().ToString();

            foreach (var item in invoice)
            {
                List<BaseServiceAppointment> lst = new List<BaseServiceAppointment>();
                lst.AddRange(item.BaseServiceAppointment.ToList());

                foreach (var service in lst)
                {
                        foreach (var loc in service.ServiceAppointmentLocations)
                        {
                            await new AdminCreateManifestHandler(_manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository, _appSetting)
                          .Create(item, ManifestStatus.Draft, loc.Id, service.StartDate);
                        }
                }
                await CreateScaduleServiceHandler.Create(item, _baseServiceAppointmentRepository, _manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository, _appSetting, locationCompanyRepository);

            }

            return new Success<CompanyContract>() { Data = entity };
        }
    }
}

