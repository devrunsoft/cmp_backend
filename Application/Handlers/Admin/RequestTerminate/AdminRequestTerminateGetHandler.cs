using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Application.Command;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminRequestTerminateGetHandler : IRequestHandler<AdminRequestTerminateGetCommand, CommandResponse<RequestTerminate>>
    {
        private readonly IRequestTerminateRepository _repository;

        public AdminRequestTerminateGetHandler(IRequestTerminateRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<RequestTerminate>> Handle(AdminRequestTerminateGetCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(x=>x.Id == request.Id, query => query
            .Include(x => x.Request)
            .ThenInclude(x => x.Company)
             .Include(x => x.Request)
            .ThenInclude(x => x.Provider)

            .Include(x => x.Request)
            .ThenInclude(x => x.BaseServiceAppointment)
            )).FirstOrDefault();
            return new Success<RequestTerminate>() { Data = result };
        }
    }
}

