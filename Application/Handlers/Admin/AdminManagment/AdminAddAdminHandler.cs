using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Handlers.Admin.AdminManagment;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminAddAdminHandler : IRequestHandler<AdminAddAdminCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _repository;
        public AdminAddAdminHandler(IAdminRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<AdminEntity>> Handle(AdminAddAdminCommand request, CancellationToken cancellationToken)
        {
            var isEsist = (await _repository.GetAsync(p=>p.Email == request.Email)).Any();

            if (isEsist)
            {
                return new NoAcess<AdminEntity>() { Message = "This email already exists." };
            }

            var person = new Person() { FirstName = request.FirstName, LastName = request.LastName, Id = Guid.NewGuid() };
            var entity = new AdminEntity
            {
            Email = request.Email,
            Role = "LimitedAdmin",
            IsActive = request.IsActive,
            Password = request.Password,
            Person = person
            };

            var result = await _repository.AddAsync(entity);
            return new Success<AdminEntity>() { Data = result };
        }
    }
}