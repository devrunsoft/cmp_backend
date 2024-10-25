using System;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class ServiceAppointmentEmergencyRepository : Repository<ServiceAppointmentEmergency, long>, IServiceAppointmentEmergencyRepository
    {
        public ServiceAppointmentEmergencyRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        public async Task<IEnumerable<ServiceAppointmentEmergency>> GetList(Expression<Func<ServiceAppointmentEmergency, bool>> expression)
        {
            return await _dbContext.ServiceAppointmentEmergency.Include(x => x.Invoice).Where(expression).ToListAsync();
        }
    }
}

