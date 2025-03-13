using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminGetAllContractHandler : IRequestHandler<AdminGetAllContractCommand, CommandResponse<PagesQueryResponse<Contract>>>
    {
        private readonly IContractRepository _repository;
        public AdminGetAllContractHandler(IContractRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<PagesQueryResponse<Contract>>> Handle(AdminGetAllContractCommand request, CancellationToken cancellationToken)
        {
               var result = (await _repository.GetBasePagedAsync(request, p =>
                    (request.Active == null || p.Active == true)
                ));

            return new Success<PagesQueryResponse<Contract>>() { Data = result };
        }
    }
}

