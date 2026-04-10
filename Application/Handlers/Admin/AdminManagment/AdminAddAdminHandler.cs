using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Handlers.Admin.AdminManagment;
using System.Linq;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application
{
    public class AdminAddAdminHandler : IRequestHandler<AdminAddAdminCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminAddAdminHandler(IAdminRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CommandResponse<AdminEntity>> Handle(AdminAddAdminCommand request, CancellationToken cancellationToken)
        {
            var isEsist = (await _repository.GetAsync(p=>p.Email == request.Email)).Any();

            if (isEsist)
            {
                return new NoAcess<AdminEntity>() { Message = "This email already exists." };
            }
            var user = _httpContextAccessor.HttpContext?.User;

            var email = user?.FindFirst("Email")?.Value;

            if (request.IsSuperAdmin && email == "alec.bagin@gmail.com")
            {
                return new NoAcess<AdminEntity>()
                {
                    Message = "Driver cannot create SuperAdmin"
                };
            }

            var person = new Person() { FirstName = request.FirstName, LastName = request.LastName, Id = Guid.NewGuid() };
            var entity = new AdminEntity
            {
            Email = request.Email,
            Role = request.IsSuperAdmin? "SuperAdmin" : "LimitedAdmin",
            IsActive = request.IsActive,
            Password = request.Password,
            Person = person,
            TwoFactor = request.TwoFactor
            };

            var result = await _repository.AddAsync(entity);
            return new Success<AdminEntity>() { Data = result };
        }
    }
}