using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CMPNatural.Application.Mapper;
using ScoutDirect.Core.Repositories.Base;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Entities;
using Stripe.Forwarding;
using System.Collections.Generic;

namespace CMPNatural.Application
{
    public class AdminCreateFinalInvoiceHandler : IRequestHandler<AdminCreateFinalInvoiceCommand, CommandResponse<InvoiceResponse>>
    {
        private readonly IinvoiceRepository _invoiceRepository;
        private readonly IManifestRepository manifestRepository;
        private readonly IMediator _mediator;
        public AdminCreateFinalInvoiceHandler(IinvoiceRepository invoiceRepository, IManifestRepository manifestRepository, IMediator _mediator)
        {
            _invoiceRepository = invoiceRepository;
            this.manifestRepository = manifestRepository;
            this._mediator = _mediator;
        }

        public async Task<CommandResponse<InvoiceResponse>> Handle(AdminCreateFinalInvoiceCommand request, CancellationToken cancellationToken)
        {

            var result = (await manifestRepository.GetAsync(p => request.ManifestIds.Any(x=> x== p.Id), query => query
            .Include(x => x.Request)
            .ThenInclude(x => x.OperationalAddress)

            .Include(x => x.Request)
            .ThenInclude(x => x.BillingInformation)

            .Include(x => x.OperationalAddress)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.LocationCompany)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.ProductPrice)

            .Include(x => x.ServiceAppointmentLocation)
            .ThenInclude(x => x.ServiceAppointment)
            .ThenInclude(x => x.Product)


            .Include(x => x.Request)
            .ThenInclude(x => x.Company)
            .Include(x => x.Provider)
            )).ToList();

            InvoiceResponse invoice;

                var errors = new List<string>();

                // Collect SALs (must exist)
                var lstCustom = result
                    .Where(m => m.ServiceAppointmentLocation != null)
                    .Select(m => m.ServiceAppointmentLocation.ServiceAppointment)
                    .ToList();

                if (lstCustom.Count == 0)
                {
                    return new NoAcess<InvoiceResponse>
                    {
                        Message = "No ServiceAppointmentLocation records found for the selected manifests."
                    };
                }

                // CompanyId consistency
                var companyIds = result
                    .Select(m => m.CompanyId)
                    .Distinct()
                    .ToList();

                if (companyIds.Count == 0)
                    errors.Add("Could not resolve CompanyId from the selected manifests.");

                if (companyIds.Count > 1)
                    errors.Add($"Selected manifests belong to multiple companies: {string.Join(", ", companyIds)}. All manifests must belong to the same Company.");

                // OperationalAddressId consistency
                var operationalIds = result
                    .Select(m => m.OperationalAddressId)
                    .Distinct()
                    .ToList();

                if (operationalIds.Count == 0)
                    errors.Add("Could not resolve OperationalAddressId from the selected manifests.");

                if (operationalIds.Count > 1)
                    errors.Add($"Selected manifests have multiple OperationalAddressIds: {string.Join(", ", operationalIds)}. All manifests must share the same OperationalAddress.");

                var billingIds = result
                    .Select(m => m.Request.BillingInformationId)
                    .Distinct()
                    .ToList();

                if (billingIds.Count == 0)
                    errors.Add("Could not resolve BillingInformationId from the selected manifests.");

                if (billingIds.Count > 1)
                    errors.Add($"Selected manifests have multiple BillingInformationIds: {string.Join(", ", billingIds)}. All manifests must share the same BillingInformation.");

                // Return combined error if anything failed
                if (errors.Count > 0)
                {
                    return new NoAcess<InvoiceResponse>
                    {
                        Message = string.Join(" | ", errors)
                    };
                }

                // We have exactly one of each
                var singleCompanyId = companyIds[0];
                var singleOperationalId = operationalIds[0];
                var singleBillingInfoId = billingIds[0];

                var invoiceId = Guid.NewGuid();

                //var resultInvoice = await _mediator.Send(new AddInvoiceSourceCommand()
                //{
                //    CompanyId = singleCompanyId,
                //    InvoiceId = invoiceId.ToString(),
                //});


                foreach (var item in result)
                {
                    item.Status = ManifestStatus.Completed;
                    item.ServiceAppointmentLocation.InvoiceId = null;
                    await manifestRepository.UpdateAsync(item);
                }

                var inv = new Invoice
                {
                    CompanyId = singleCompanyId,
                    Status = InvoiceStatus.Draft,
                    BaseServiceAppointment = lstCustom,
                    Address = result.FirstOrDefault().OperationalAddress.Address,
                    OperationalAddressId = singleOperationalId,
                    CreatedAt = DateTime.UtcNow,
                    BillingInformationId = singleBillingInfoId,
                    Type = InvoiceType.Final_Invoice,
                    InvoiceId = invoiceId.ToString(),
                    InvoiceNumber = invoiceId.ToString(),
                    RequestNumber = "",
                    ContractId = result.First().ContractId,
                    SendDate = DateTime.Now,
                    RequestId= result.FirstOrDefault().RequestId,
                };

                inv.CalculateAmount();

                inv = await _invoiceRepository.AddAsync(inv);
                inv.InvoiceNumber = inv.Number;
                await _invoiceRepository.UpdateAsync(inv);

                invoice = InvoiceMapper.Mapper.Map<InvoiceResponse>(inv);
            
            


            return new Success<InvoiceResponse>() { Data = invoice };
        }
    }
}

