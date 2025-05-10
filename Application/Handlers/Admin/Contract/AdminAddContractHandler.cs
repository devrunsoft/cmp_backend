using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application
{
    public class AdminAddContractHandler : IRequestHandler<AdminAddContractCommand, CommandResponse<Contract>>
    {
        private readonly IContractRepository _repository;
        public AdminAddContractHandler(IContractRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<Contract>> Handle(AdminAddContractCommand request, CancellationToken cancellationToken)
        {
            var entity = new Contract
            {
              Active = request.Active,
              Content = request.Content,
              Title = request.Title,
              CreatedAt = DateTime.Now,
              IsDefault = request.IsDefault
            };

            if (request.IsDefault)
            {
                await _repository.UnsetDefaultForOthersAsync(0);
            }

            var result = await _repository.AddAsync(entity);
            return new Success<Contract>() { Data = result };
        }
    }
}

