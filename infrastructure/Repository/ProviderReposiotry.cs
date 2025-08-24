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
        public ProviderReposiotry(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService)
            : base(context, cacheService) { }

        // Only include ServiceArea unless ProviderService is needed
        private IQueryable<Provider> GeBasicQuery() =>
            _dbContext.Provider
                .Include(p => p.ServiceArea)
                .AsNoTracking(); // Disables change tracking for performance

        public async Task<List<Provider>> GetAllSearchProviderAsync(
            double sLatitude,
            double sLongitude,
            Expression<Func<Provider, bool>> expression)
        {
            // Fetch only filtered providers from DB
            var providerList = await GeBasicQuery()
                .Where(expression)
                .ToListAsync();

            // Filter in-memory using optimized geo logic (with parallel)
            var result = providerList
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Where(p => p.IsPointInCity(sLatitude, sLongitude))
                .OrderBy(p => p.Id)
                .ToList();

            return result;
        }

        public async Task<List<Provider>> GetAllSearchProviderAllInvoiceAsync(List<ServiceAppointmentLocation> services, bool checkLocation = true)
        {
            // Include ProviderService only here
            var providerList = await _dbContext.Provider
                .Include(p => p.ServiceArea)
                .Include(p => p.ProviderService)
                .Where(p => p.Status != CMPNatural.Core.Enums.ProviderStatus.Blocked)
                .AsNoTracking()
                .ToListAsync();

            var result = providerList
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Where(p =>
                    (!checkLocation || services.Any(loc =>
                        p.IsPointInCity(loc.LocationCompany.Lat, loc.LocationCompany.Long))) &&
                    services.All(loc =>
                        p.ProviderService.Any(service => service.ProductId == loc.ServiceAppointment.ProductId))
                )
                .ToList();

            return result;
        }
    }
}
