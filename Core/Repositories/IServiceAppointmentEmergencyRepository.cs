using System;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface IServiceAppointmentEmergencyRepository : IRepository<ServiceAppointmentEmergency, long>
    {
        Task<IEnumerable<ServiceAppointmentEmergency>> GetList(Expression<Func<ServiceAppointmentEmergency, bool>> expression);
    }
}

