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
    public class ServiceAppointmentRepository : Repository<ServiceAppointment, long>, IServiceAppointmentRepository
    {
        public ServiceAppointmentRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }

        public async Task<IEnumerable<ServiceAppointment>> GetList(Expression<Func<ServiceAppointment, bool>> expression)
        {
            return await _dbContext.ServiceAppointment.Where(expression).ToListAsync();
        }
    }
}

