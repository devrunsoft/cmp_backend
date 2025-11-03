using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Models;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{
    public class AdminProviderSubmitInvoiceHandler : IRequestHandler<AdminProviderSubmitInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IManifestRepository _repository;
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IAppInformationRepository _apprepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository;
        private readonly AppSetting _appSetting;

        public AdminProviderSubmitInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository, IManifestRepository _repository, IAppInformationRepository _apprepository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, AppSetting appSetting)
        {
            _invoiceRepository = invoiceRepository;
            _productPriceRepository = productPriceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._repository = _repository;
            this._apprepository = _apprepository;
            this._serviceAppointmentLocationRepository = _serviceAppointmentLocationRepository;
            this._appSetting = appSetting;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminProviderSubmitInvoiceCommand requests, CancellationToken cancellationToken)
        {
            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId,
                query => query.Include(x => x.Company)
                )).FirstOrDefault();
            //var entity = (await _repository.GetAsync(x => x.RequestId == invoice.RequestId)).FirstOrDefault();

            if (invoice.Status == InvoiceStatus.Send_Payment)
            {
                return new NoAcess<InvoiceResponse>() { Message = "No Access To Edit This Invoice" };
            }

            if (invoice.Status != InvoiceStatus.Draft && invoice.Status != InvoiceStatus.Submited_Provider && invoice.Status != InvoiceStatus.Updated_Provider && invoice.Status != InvoiceStatus.Processing_Provider && invoice.Status != InvoiceStatus.Send_To_Admin)
            {
                return new NoAcess<InvoiceResponse>
                {
                    Message = "You cannot edit this invoice because it has not been submitted by the provider."
                };
            }

            var CompanyId = invoice.CompanyId;
            var services = (await _baseServiceAppointmentRepository.GetAsync(p => true)).ToList();

            foreach (var srv in services)
            {
                srv.Status = ServiceStatus.Complete;
                await _baseServiceAppointmentRepository.UpdateAsync(srv);
            }

            invoice.Status = InvoiceStatus.Send_Payment;
            invoice.CalculateAmountByamount();
            await _invoiceRepository.UpdateAsync(invoice);

            var information = (await _apprepository.GetAllAsync()).LastOrDefault();
            //await CreateManifestContent.CreateContent(invoice, information, entity, _serviceAppointmentLocationRepository, _appSetting);
            //entity.Content = "";
            //entity.Status = ManifestStatus.Completed;
            //entity.Comment = requests.Comment;
            //entity.ManifestNumber = entity.Number;
            //await _repository.UpdateAsync(entity);

            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(invoice) };

        }
    }
}
