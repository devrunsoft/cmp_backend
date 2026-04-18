using System;
using CMPNatural.Core.Entities;
using ScoutDirect.Application.Responses;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using CMPNatural.Api.Service;

namespace CMPNatural.Application
{
	public class AdminCreateManifestHandler
	{
        private readonly IRequestRepository _invoiceRepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IManifestRepository _repository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly AppSetting _appSetting;
        public AdminCreateManifestHandler(IManifestRepository _repository, IRequestRepository _invoiceRepository, IAppInformationRepository _apprepository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, AppSetting appSetting, IServiceScopeFactory serviceScopeFactory)
        {

            this._invoiceRepository = _invoiceRepository;
            this._apprepository = _apprepository;
            this._repository = _repository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
            this._appSetting = appSetting;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<CommandResponse<Manifest>> Create(RequestEntity request , ManifestStatus status , long ServiceAppointmentLocationId, DateTime StartDate, long adminId, MessageNoteType messageNoteType)
		{
            //var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            //var services = (await _invoiceRepository.GetAsync(x => x.Id == invoice.Id, query => query
            //    .Include(x => x.BaseServiceAppointment)
            //    .ThenInclude(x => x.Product)
            //    .Include(x => x.BaseServiceAppointment)
            //    .ThenInclude(x => x.ProductPrice)
            //    )).FirstOrDefault();

            var entity = new Manifest()
            {
                RequestId = request.Id,
                Status = status,
                Content = "",
                ContractId = request.ContractId.Value,
                CompanyId = request.CompanyId,
                CreatedAt = request.CreatedAt,
                ManifestNumber = "",
                OperationalAddressId = request.OperationalAddressId,
                ServiceAppointmentLocationId = ServiceAppointmentLocationId,
                PreferredDate = StartDate,
            };

            var result = await _repository.AddAsync(entity);
            //await CreateManifestContent.CreateContent(services, information, entity, _serviceAppointmentLocationRepository, _appSetting);
            entity.Content = "";
            entity.ManifestNumber = entity.Number;
            await _repository.UpdateAsync(entity);

            new Note(adminId, serviceScopeFactory).adminSendNote(messageNoteType , request.CompanyId, request.OperationalAddressId, entity , entity.NoteTitle);

            return new Success<Manifest>() { Data = result};

        }
    }
}

