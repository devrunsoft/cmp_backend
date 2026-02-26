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
using CMPNatural.Core.Extentions;

namespace CMPNatural.Application
{
    public class AdminSignProviderContractHandler : IRequestHandler<AdminSignProviderContractCommand, CommandResponse<ProviderContract>>
    {
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IProviderContractRepository _repository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IRequestRepository _invoiceRepository;
        private readonly IRequestRepository requestRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly AppSetting _appSetting;
        private readonly ILocationCompanyRepository locationCompanyRepository;

        public AdminSignProviderContractHandler(IProviderContractRepository repository, IRequestRepository invoiceRepository, IRequestRepository requestRepository, IAppInformationRepository apprepository,
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
        public async Task<CommandResponse<ProviderContract>> Handle(AdminSignProviderContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.ProviderContractId);
            var appinformation = (await _apprepository.GetAllAsync()).LastOrDefault();
            if (appinformation == null || appinformation.Sign == null)
            {
                return new NoAcess<ProviderContract>() { Message = "Please add a sign in Company Information" };
            }

            if (entity.Status != CompanyContractStatus.Needs_Admin_Signature)
            {
                return new NoAcess<ProviderContract>
                {
                    Message = "Contract cannot be signed at this stage. It must be in 'Needs Admin Signature' status."
                };
            }

            string formattedTime = DateTime.Now.ToString("hh:mm tt");

            var content = entity.Content;
            content = CompanyContractHelper.ShowByKey(ContractProviderKeysEnum.ManagmentCompanySign.GetDescription(), content, CompanyContractHelper.SignFont);
            content = content.Replace(ContractProviderKeysEnum.ManagmentCompanySign.GetDescription(), appinformation.Sign);
            content = CompanyContractHelper.ShowByKey(ContractProviderKeysEnum.ManagmentCompanySignDateTime.GetDescription(), content);
            content = content.Replace(ContractProviderKeysEnum.ManagmentCompanySignDateTime.GetDescription(), $"{DateTime.Now.ToDateString()} {formattedTime}");

            entity.Content = content;
            entity.Status = CompanyContractStatus.Signed;
            entity.AdminSign = appinformation.Sign;
            entity.AdminSignDate = DateTime.Now;
            await _repository.UpdateAsync(entity);

            //update Invoice
            //var invoice = await requestRepository.GetAsync(x => x.Id == entity.RequestId && x.Status == InvoiceStatus.Needs_Admin_Signature &&
            //x.CompanyId == request.CompanyId, query => query
            //.Include(x => x.BaseServiceAppointment)
            //.ThenInclude(x => x.ServiceAppointmentLocations)

            //.Include(x => x.BaseServiceAppointment)
            //.ThenInclude(x => x.ProductPrice)

            //.Include(x => x.BaseServiceAppointment)
            //.ThenInclude(x => x.Product)
            //);
            //var invid = Guid.NewGuid().ToString();

            //foreach (var item in invoice)
            //{
            //    List<BaseServiceAppointment> lst = new List<BaseServiceAppointment>();
            //    lst.AddRange(item.BaseServiceAppointment.ToList());

            //    foreach (var service in lst)
            //    {
            //        foreach (var loc in service.ServiceAppointmentLocations)
            //        {
            //            await new AdminCreateManifestHandler(_manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository, _appSetting)
            //          .Create(item, ManifestStatus.Draft, loc.Id, service.StartDate);
            //        }
            //    }
            //    await CreateScaduleServiceHandler.Create(item, _baseServiceAppointmentRepository, _manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository, _appSetting, locationCompanyRepository);

            //}

            return new Success<ProviderContract>() { Data = entity };
        }
    }
}

