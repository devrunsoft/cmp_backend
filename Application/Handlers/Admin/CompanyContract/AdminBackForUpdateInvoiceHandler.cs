using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin.Invoice;
using CMPNatural.Application.Handlers;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{
    public class AdminBackForUpdateInvoiceHandler : IRequestHandler<AdminBackForUpdateInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IBaseServiceAppointmentRepository _baseServiceAppointmentRepository;
        private readonly IContractRepository _contractRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        private readonly IAppInformationRepository _appRepository;

        public AdminBackForUpdateInvoiceHandler(IinvoiceRepository invoiceRepository, IProductPriceRepository productPriceRepository ,
             IBaseServiceAppointmentRepository baseServiceAppointmentRepository, IContractRepository _contractRepository,
             ICompanyContractRepository _companyContractRepository, IAppInformationRepository _appRepository)
        {
            _invoiceRepository = invoiceRepository;
            _productPriceRepository = productPriceRepository;
            _baseServiceAppointmentRepository = baseServiceAppointmentRepository;
            this._contractRepository = _contractRepository;
            this._companyContractRepository = _companyContractRepository;
            this._appRepository = _appRepository;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminBackForUpdateInvoiceCommand requests, CancellationToken cancellationToken)
        {

            var invoice = (await _invoiceRepository.GetAsync(p => p.Id == requests.InvoiceId)).FirstOrDefault();
            var companyContract = (await _companyContractRepository.GetAsync(p => p.Id == requests.CompanyContractId)).FirstOrDefault();
            if (invoice.Status != InvoiceStatus.Pending_Signature)
            {
                return new NoAcess<InvoiceResponse>() { Message = "No Access To Edit this Request" };
            }

            invoice.ContractId = null;
            invoice.Status = InvoiceStatus.Draft;
            await _invoiceRepository.UpdateAsync(invoice);
            await _companyContractRepository.DeleteAsync(companyContract);

            return new Success<InvoiceResponse>() { Data = InvoiceMapper.Mapper.Map<InvoiceResponse>(invoice), Message = "Successfull!" };

        }

    }
}

