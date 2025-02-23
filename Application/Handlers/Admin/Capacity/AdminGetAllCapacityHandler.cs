using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application
{
    public class AdminGetAllCapacityHandler : IRequestHandler<AdminGetAllCapacityCommand, CommandResponse<PagesQueryResponse<Capacity>>>
    {
        private readonly ICapacityRepository _repository;

        public AdminGetAllCapacityHandler(ICapacityRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }

        public async Task<CommandResponse<PagesQueryResponse<Capacity>>> Handle(AdminGetAllCapacityCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _repository.GetBasePagedAsync(request,p=> request.Enable != null? p.Enable : true ,null));
            return new Success<PagesQueryResponse<Capacity>>() { Data = invoices };
        }
    }
}

