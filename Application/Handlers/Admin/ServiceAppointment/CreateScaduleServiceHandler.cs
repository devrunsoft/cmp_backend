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
using CMPNatural.Core.Models;
using Stripe;

namespace CMPNatural.Application
{
    public class CreateScaduleServiceHandler
    {
        public static async Task Create(
            RequestEntity invoiceTemplate, IBaseServiceAppointmentRepository _serviceAppointment, IManifestRepository _manifestRepository, IRequestRepository _invoiceRepository, IAppInformationRepository _apprepository,
             IServiceAppointmentLocationRepository _serviceAppointmentLocationRepository, AppSetting _appSetting, ILocationCompanyRepository locationCompanyRepository)
        {

            var request = (await _invoiceRepository.GetAsync(x=> x.Id == invoiceTemplate.Id,
                query => query
                .Include(i => i.BaseServiceAppointment)
                .ThenInclude(i => i.ProductPrice)
                .ThenInclude(p => p.Product)
                )).FirstOrDefault();

            var baseAppointments = request.BaseServiceAppointment.ToList();
            var allAppointments = new List<BaseServiceAppointment>();

            var locations = (await locationCompanyRepository.GetAsync(x => x.CompanyId == request.CompanyId,
            query => query.Include(x => x.CapacityEntity)
            )).ToList();

            // Step 1: Generate all future appointments
            foreach (var item in baseAppointments)
            {
                var numberOfPayments = item.ProductPrice.NumberofPayments;
                numberOfPayments = numberOfPayments == 0 ? 1 : numberOfPayments;
                var intervalDays = 365 / numberOfPayments;
                var baseStartDate = item.StartDate;

                for (int i = 1; i < numberOfPayments; i++)
                {
                    var status = i == 0 ? ServiceStatus.UpComingScaduled : ServiceStatus.Scaduled;
                    var appointmentDate = baseStartDate.AddDays(i * intervalDays);
                    var appointment = new BaseServiceAppointment
                    {
                        Id = 0,
                        RequestId = invoiceTemplate.Id,
                        CompanyId = item.CompanyId,
                        ProductId = item.ProductId,
                        ProductPrice = item.ProductPrice,
                        ProductPriceId = item.ProductPriceId,
                        Product = item.Product,
                        StartDate = appointmentDate,
                        OperationalAddressId = item.OperationalAddressId,
                        Status = status,
                        IsEmegency = item.IsEmegency,
                        Qty = item.Qty,
                        //FactQty = item.FactQty,
                        Amount = item.Amount,
                        ServiceTypeId = item.Product.ServiceType,
                        ServiceAppointmentLocations = item.ServiceAppointmentLocations
                           .Select(id =>
                           {
                               var locationCompany = locations.FirstOrDefault(l => l.Id == id.LocationCompanyId);
                               return new ServiceAppointmentLocation
                               {
                                   LocationCompanyId = id.LocationCompanyId,
                                   Qty = locationCompany?.CapacityEntity?.Qty ?? 1,
                                   Status = status
                               };
                           }).ToList(),
                        DayOfWeek = item.DayOfWeek,
                        FromHour = item.FromHour,
                        ToHour = item.ToHour,
                        ScaduleDate = appointmentDate,
                    };
                    appointment = await _serviceAppointment.AddAsync(appointment);

                    allAppointments.Add(appointment);
                }
            }


            foreach (var service in allAppointments)
            {
                foreach (var loc in service.ServiceAppointmentLocations)
                {

                    await new AdminCreateManifestHandler(_manifestRepository, _invoiceRepository, _apprepository, _serviceAppointmentLocationRepository, _appSetting)
                        .Create(request, ManifestStatus.Scaduled, loc.Id, service.StartDate);

                }

            }
        }

    }
}

