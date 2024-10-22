using System;
using System.Linq.Expressions;
using CMPNatural.Core.Entities;
using ScoutDirect.Core.Repositories.Base;

namespace CMPNatural.Core.Repositories
{
    public interface IServiceAppointmentRepository : IRepository<ServiceAppointment, long>
    {
        Task<IEnumerable<ServiceAppointment>> GetList(Expression<Func<ServiceAppointment, bool>> expression);
    }
}

