using System;
using System.Linq;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application.Handlers
{
	public class CancelService
	{
        private readonly IBaseServiceAppointmentRepository _serviceAppointmentRepository;
        private readonly IManifestRepository _manifestRepository;
        private readonly IinvoiceRepository _iinvoiceRepository;
        private readonly ICompanyContractRepository _companyContractRepository;
        public CancelService(IBaseServiceAppointmentRepository _serviceAppointmentRepository, IManifestRepository _manifestRepository,
            IinvoiceRepository _iinvoiceRepository, ICompanyContractRepository _companyContractRepository)
        {
            this._serviceAppointmentRepository = _serviceAppointmentRepository;
            this._manifestRepository = _manifestRepository;
            this._iinvoiceRepository = _iinvoiceRepository;
            this._companyContractRepository = _companyContractRepository;
        }

        public async Task<BaseServiceAppointment> cancel(BaseServiceAppointment model, CancelEnum cancelEnum)
        {
            var result = (await _serviceAppointmentRepository.GetAsync(p => p.Id == model.Id,query=>
            query.Include(x=>x.Invoice)
            )).FirstOrDefault();

            var manifest = (await _manifestRepository.GetAsync(p => p.InvoiceId == result.InvoiceId)).FirstOrDefault();
            //var contract = (await _companyContractRepository.GetAsync(p => p.InvoiceId == result.Invoice.InvoiceId)).FirstOrDefault();

            //contract.Status = (int)CompanyContractStatus.Canceld;
            //await _companyContractRepository.UpdateAsync(contract);


            var canCacnelInvoice = (await _serviceAppointmentRepository.GetAsync(p => p.InvoiceId == result.InvoiceId && p.Id!=result.Id &&
            (p.Status == ServiceStatus.Draft || p.Status == ServiceStatus.Scaduled || p.Status == ServiceStatus.Proccessing)
            )).ToList();

            if (!canCacnelInvoice.Any())
            {
                manifest.Status = ManifestStatus.Canceled;
                await _manifestRepository.UpdateAsync(manifest);
                result.Invoice.Status = InvoiceStatus.Canceled;
            }


            result.Status = ServiceStatus.Canceled;
            result.CancelBy = cancelEnum;

            await _serviceAppointmentRepository.UpdateAsync(result);


            return result;

        }
    }

}

