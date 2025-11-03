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

namespace CMPNatural.Application
{
    public class AdminRequestTerminateGetAllHandler : IRequestHandler<AdminRequestTerminateGetAllCommand, CommandResponse<PagesQueryResponse<RequestTerminate>>>
    {
        private readonly IRequestTerminateRepository _repository;

        public AdminRequestTerminateGetAllHandler(IRequestTerminateRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<RequestTerminate>>> Handle(AdminRequestTerminateGetAllCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, null, query => query
            .Include(x => x.Request)
            .ThenInclude(x => x.Company)
             .Include(x => x.Request)
            .ThenInclude(x => x.Provider)
            ));
            return new Success<PagesQueryResponse<RequestTerminate>>() { Data = result };
        }
    }
}

