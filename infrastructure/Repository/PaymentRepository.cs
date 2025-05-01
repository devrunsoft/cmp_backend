using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class PaymentRepository : Repository<Payment, long>, IPaymentRepository
    {
        public PaymentRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService) : base(context, cacheService) { }
    }

}
