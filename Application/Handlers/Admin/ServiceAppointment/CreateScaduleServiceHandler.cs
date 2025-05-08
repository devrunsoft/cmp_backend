using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using CMPNatural.Core.Enums;
using Stripe.Forwarding;
using MediatR;
using System.Linq;
using ScoutDirect.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;

namespace CMPNatural.Application
{
    public class CreateScaduleServiceHandler
    {
        public static async Task Create(
            Invoice invoiceTemplate, IManifestRepository _manifestRepository, IinvoiceRepository _invoiceRepository, IAppInformationRepository _apprepository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository)
        {

            var invoice = (await _invoiceRepository.GetAsync(x=> x.Id == invoiceTemplate.Id,
                query => query
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ProductPrice)
                .ThenInclude(p => p.Product)
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ServiceAppointmentLocations)
                )).FirstOrDefault();

            var baseAppointments = invoice.BaseServiceAppointment;
            var allAppointments = new List<BaseServiceAppointment>();

            // Step 1: Generate all future appointments
            foreach (var item in baseAppointments)
            {
                var numberOfPayments = item.ProductPrice.NumberofPayments;
                var intervalDays = 365 / numberOfPayments;
                var baseStartDate = item.StartDate;

                for (int i = 1; i < numberOfPayments; i++)
                {
                    var appointmentDate = baseStartDate.AddDays(i * intervalDays);
                    var appointment = new BaseServiceAppointment
                    {
                        Id = 0,
                        CompanyId = item.CompanyId,
                        ProductId = item.ProductId,
                        ProductPrice = item.ProductPrice,
                        ProductPriceId = item.ProductPriceId,
                        Product = item.Product,
                        StartDate = appointmentDate,
                        OperationalAddressId = item.OperationalAddressId,
                        Status = i == 0 ? ServiceStatus.UpComingScaduled : ServiceStatus.Scaduled,
                        IsEmegency = item.IsEmegency,
                        Qty = item.Qty,
                        FactQty = item.FactQty,
                        Amount = item.Amount,
                        ServiceTypeId = item.Product.ServiceType,
                        ServiceAppointmentLocations = item.ServiceAppointmentLocations.Select(x=> new ServiceAppointmentLocation()
                        {
                            LocationCompanyId = x.LocationCompanyId,
                        }).ToList(),
                        DayOfWeek = item.DayOfWeek,
                        FromHour = item.FromHour,
                        ToHour = item.ToHour,
                        ScaduleDate = appointmentDate
                    };

                    allAppointments.Add(appointment);
                }
            }

            // Step 2: Group appointments by StartDate (same invoice if same date)
            var groupedAppointments = allAppointments
                .GroupBy(a => a.StartDate.Date);

            // Step 3: Create invoices for each group
            foreach (var group in groupedAppointments)
            {
                var groupedInvoice = new Invoice
                {
                    CompanyId = invoice.CompanyId,
                    Status = InvoiceStatus.Scaduled,
                    InvoiceId = invoice.InvoiceId,
                    BaseServiceAppointment = group.ToList(),
                    Amount = group.Sum(a => a.Amount),
                    Address = invoice.Address,
                    OperationalAddressId = invoice.OperationalAddressId,
                    ContractId = invoice.ContractId,
                    SendDate = group.FirstOrDefault().StartDate,
                    CreatedAt = DateTime.Now,
                    InvoiceNumber = ""
                };
                groupedInvoice.CalculateAmount();

               var item = await _invoiceRepository.AddAsync(groupedInvoice);
               item.InvoiceNumber = invoice.Number;
               await _invoiceRepository.UpdateAsync(item);
                await new AdminCreateManifestHandler(_manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository).Create(item, ManifestStatus.Scaduled);
            }
        }

    }
}

