using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Base;

namespace CMPNatural.Application.Handlers.CommandHandlers.Service
{

    public class GetAllProductPaginateHandler : IRequestHandler<GetAllProductPaginateCommand, CommandResponse<PagesQueryResponse<Product>>>
    {
        private readonly IProductRepository _repository;
        public GetAllProductPaginateHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<Product>>> Handle(GetAllProductPaginateCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetBasePagedAsync(request , p =>
                    (request.Enable == null || p.Enable == request.Enable) &&
                    (request.ServiceType == null || p.ServiceType == (int)request.ServiceType) &&
                    (request.Type == null || p.Type == (int)request.Type)
                ));
            return new Success<PagesQueryResponse<Product>>() { Data = result };
        }
    }
}

