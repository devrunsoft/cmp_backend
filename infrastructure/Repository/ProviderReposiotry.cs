using System;
using System.Linq;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Base;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class ProviderReposiotry : Repository<Provider, long>, IProviderReposiotry
    {
        public ProviderReposiotry(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        private IQueryable<Provider> GeAllQuery() => _dbContext.Provider.Include(p=>p.ProviderService).Include(x=>x.ServiceArea);

        public async Task<List<Provider>> GetAllSearchProviderAsync(double sLatitude, double sLongitude, Expression<Func<Provider, bool>> expression)
        {
            var cachedList = await GeAllQuery().Where(expression)
             .ToListAsync();

            return cachedList.Where(p =>  p.IsPointInCity(sLatitude, sLongitude)
             )
                //.OrderBy(p => p.Distance(sLatitude, sLongitude))
                .OrderBy(p => p.Id).ToList();
        }


        public async Task<List<Provider>> GetAllSearchProviderAllInvoiceAsync(List<ServiceAppointmentLocation> locations)
        {
            var cachedList = await GeAllQuery()
            .ToListAsync();
            return cachedList
               .Where(p =>
               p.Status != Core.Enums.ProviderStatus.Blocked &&
                  locations.Any(s => p.IsPointInCity(s.LocationCompany.Lat, s.LocationCompany.Long)) &&
                  locations.All(loc => p.ProviderService.Any(service => service.ProductId == loc.ServiceAppointment.ProductId)) // Ensure all exist
             ).ToList();
        }
    }
}

