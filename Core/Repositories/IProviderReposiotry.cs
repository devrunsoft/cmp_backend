using System;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Base;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface IProviderReposiotry : IRepository<Provider, long>
    {
        Task<List<Provider>> GetAllSearchProviderAsync(double sLatitude, double sLongitude);
        Task<List<Provider>> GetAllSearchProviderAllInvoiceAsync(List<ServiceAppointmentLocation> locations);
    }
}

