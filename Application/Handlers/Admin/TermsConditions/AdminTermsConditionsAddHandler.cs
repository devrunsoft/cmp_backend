using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace CMPNatural.Application
{
    public class AdminTermsConditionsAddHandler : IRequestHandler<AdminTermsConditionsAddCommand, CommandResponse<TermsConditions>>
    {
        private readonly ITermsConditionsRepository _repository;
        public AdminTermsConditionsAddHandler(ITermsConditionsRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<TermsConditions>> Handle(AdminTermsConditionsAddCommand request, CancellationToken cancellationToken)
        {
            var entity = new TermsConditions
            {
              Content = request.Content,
              CreateAt = DateTime.Now,
              Enable = request.Enable,
              Type = request.Type
            };
            var result = await _repository.AddAsync(entity);
            return new Success<TermsConditions>() { Data = result };
        }
    }
}

