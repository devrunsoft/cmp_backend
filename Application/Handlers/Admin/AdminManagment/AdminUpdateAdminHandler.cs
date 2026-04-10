using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CMPNatural.Application
{
    public class AdminUpdateAdminHandler : IRequestHandler<AdminUpdateAdminCommand, CommandResponse<AdminEntity>>
    {
        private readonly IAdminRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminUpdateAdminHandler(IAdminRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CommandResponse<AdminEntity>> Handle(AdminUpdateAdminCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAsync(p=>p.Id== request.Id, query=>query.Include(x=>x.Person))).FirstOrDefault();

            var user = _httpContextAccessor.HttpContext?.User;

            var email = user?.FindFirst("Email")?.Value;

            if (request.IsSuperAdmin && (email != "alec.bagin@gmail.com" && email != "devrunsoft@gmail.com"))
            {
                return new NoAcess<AdminEntity>()
                {
                    Message = "Access denied."
                };
            }else if (!request.IsSuperAdmin && entity.Role == "SuperAdmin" && (email != "alec.bagin@gmail.com" && email != "devrunsoft@gmail.com"))
            {
                return new NoAcess<AdminEntity>()
                {
                    Message = "Access denied."
                };
            }

            entity.Role = request.IsSuperAdmin ? "SuperAdmin" : "LimitedAdmin";
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

