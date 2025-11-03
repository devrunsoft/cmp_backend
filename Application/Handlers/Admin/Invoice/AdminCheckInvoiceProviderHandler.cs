using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using Stripe;

namespace CMPNatural.Application
{

    public class AdminCheckInvoiceProviderHandler : IRequestHandler<AdminCheckInvoiceProviderCommand, CommandResponse<List<Provider>>>
    {
        private readonly IManifestRepository manifestRepository;
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminCheckInvoiceProviderHandler(IManifestRepository manifestRepository, IProviderReposiotry providerReposiotry)
        {
            this.manifestRepository = manifestRepository;
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<List<Provider>>> Handle(AdminCheckInvoiceProviderCommand request, CancellationToken cancellationToken)
        {
            //int MaxDistance = 2000; //meter
            var result = (await manifestRepository.GetAsync(p=>p.Id== request.ManifestId,
                query => query
                .Include(i => i.ServiceAppointmentLocation)
                .ThenInclude(i => i.ServiceAppointment)
                .ThenInclude(i => i.Product)
                .Include(i => i.ServiceAppointmentLocation)
                .ThenInclude(i => i.LocationCompany)

                )).FirstOrDefault();

            bool shouldCheckLocation = false;

            //var locations = invoices.ServiceAppointmentLocation;
            //{
            //    if (p.Product.ServiceType == (int)ServiceType.Cooking_Oil_Collection || p.Product.ServiceType == (int)ServiceType.Grease_Trap_Management)
            //    {
            //        shouldCheckLocation = true;
            //    }
            //    return p.ServiceAppointmentLocations;
            //};
            var p = result.ServiceAppointmentLocation.ServiceAppointment;

            if (p.Product.ServiceType == (int)ServiceType.Cooking_Oil_Collection || p.Product.ServiceType == (int)ServiceType.Grease_Trap_Management)
            {
                shouldCheckLocation = true;
            }

            var providers = (await _providerReposiotry
           .GetAllSearchProviderAllInvoiceAsync(new List<ServiceAppointmentLocation>() { result.ServiceAppointmentLocation }, shouldCheckLocation)).ToList();

            return new Success<List<Provider>>() { Data = providers };
        }
    }
}
