using System;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;

namespace CMPNatural.Application
{
    public class AdminEditContractHandler : IRequestHandler<AdminEditContractCommand, CommandResponse<Contract>>
    {
        private readonly IContractRepository _repository;
        public AdminEditContractHandler(IContractRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<Contract>> Handle(AdminEditContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            entity.Active = request.Active;
            entity.Content = request.Content;
            entity.Title = request.Title;

            await _repository.UpdateAsync(entity);
            return new Success<Contract>() { Data = entity };
        }
    }
}

