using System;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using ScoutDirect.Core.Repositories;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Repositories;
using System.Linq;
using ScoutDirect.Core.Caching;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class DriverLoginHandler : IRequestHandler<DriverLoginCommand, CommandResponse<DriverResponse>>
    {
        private readonly IDriverRepository _repository;

        public DriverLoginHandler(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<DriverResponse>> Handle(DriverLoginCommand request, CancellationToken cancellationToken)
        {
            var admin = (await _repository.GetAsync(p => p.Email == request.Email, query => query.Include(x => x.Person))).FirstOrDefault();

            if (admin == null)
            {
                return new NoAcess<DriverResponse>() { Message = "Login failed. Please check your username and password and try again." };
            }
            if (admin.Status == DriverStatus.Blocked)
            {
                return new NoAcess<DriverResponse>() { Message = "Your account is inactive. Please contact support for assistance." };
            }
            if (admin.Password != request.Password)
            {
                return new NoAcess<DriverResponse>() { Message = "Login failed. Please check your username and password and try again." };
            }

            return new Success<DriverResponse>() { Data = DriverMapper.Mapper.Map<DriverResponse>(admin) };

        }

    }
}

