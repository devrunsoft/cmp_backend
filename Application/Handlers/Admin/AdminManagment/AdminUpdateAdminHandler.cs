using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminUpdateAdminHandler : IRequestHandler<AdminUpdateAdminCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _repository;
        public AdminUpdateAdminHandler(IAdminRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<AdminEntity>> Handle(AdminUpdateAdminCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=>p.Id== request.Id, query=>query.Include(x=>x.Person))).FirstOrDefault();

            entity.Person.FirstName = request.FirstName;
            entity.Person.LastName = request.LastName;
            entity.Email = request.Email;
            entity.IsActive = request.IsActive;
            entity.Password = request.Password;
            entity.TwoFactor = request.TwoFactor;

             await _repository.UpdateAsync(entity);
            return new Success<AdminEntity>() { Data = entity };
        }
    }
}

