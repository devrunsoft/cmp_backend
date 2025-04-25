using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMPNatural.Application
{
    public class AdminTermsConditionsDeleteHandler : IRequestHandler<AdminTermsConditionsDeleteCommand, CommandResponse<TermsConditions>>
    {
        private readonly ITermsConditionsRepository _repository;
        public AdminTermsConditionsDeleteHandler(ITermsConditionsRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<TermsConditions>> Handle(AdminTermsConditionsDeleteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            await _repository.DeleteAsync(entity);
            return new Success<TermsConditions>() { Data = entity };
        }
    }
}

