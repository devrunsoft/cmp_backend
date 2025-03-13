using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminAppInformationGetHandler : IRequestHandler<AdminAppInformationGetCommand, CommandResponse<AppInformation>>
    {
        private readonly IAppInformationRepository _repository;
        public AdminAppInformationGetHandler(IAppInformationRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<AppInformation>> Handle(AdminAppInformationGetCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAllAsync()).FirstOrDefault();
            return new Success<AppInformation>() { Data = entity };
        }
    }
}

