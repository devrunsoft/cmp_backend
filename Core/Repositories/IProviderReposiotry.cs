using System;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Base;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface IProviderReposiotry : IRepository<Provider, long>
    {
        Task<List<Provider>> GetAllSearchProviderAsync(double sLatitude, double sLongitude, Expression<Func<Provider, bool>> expression);
        Task<List<Provider>> GetAllSearchProviderAllInvoiceAsync(List<ServiceAppointmentLocation> locations);
    }
}

