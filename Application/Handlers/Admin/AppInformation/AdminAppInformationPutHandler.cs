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
    public class AdminAppInformationPutHandler : IRequestHandler<AdminAppInformationPutCommand, CommandResponse<AppInformation>>
    {
        private readonly IAppInformationRepository _repository;
        public AdminAppInformationPutHandler(IAppInformationRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<AppInformation>> Handle(AdminAppInformationPutCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAllAsync()).FirstOrDefault();

            entity.CompanyTitle = request.CompanyTitle;

            await _repository.UpdateAsync(entity);
            return new Success<AppInformation>() { Data = entity };

        }
    }
}

