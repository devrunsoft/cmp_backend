using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Command;

namespace CMPNatural.Application
{
    public class ClientRequestTerminateGetAllHandler : IRequestHandler<ClientRequestTerminateGetAllCommand, CommandResponse<PagesQueryResponse<RequestTerminate>>>
    {
        private readonly IRequestTerminateRepository _repository;

        public ClientRequestTerminateGetAllHandler(IRequestTerminateRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<RequestTerminate>>> Handle(ClientRequestTerminateGetAllCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request, x=>x.CompanyId == request.CompanyId &&
            (request.OperationalAddressId == 0 || x.OperationalAddressId == request.OperationalAddressId),
            query => query
            .Include(x => x.Invoice)
            .ThenInclude(x => x.Company)
             .Include(x => x.Invoice)
            .ThenInclude(x => x.Provider)
            ));
            return new Success<PagesQueryResponse<RequestTerminate>>() { Data = result };
        }
    }
}

