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
    public class AdminTermsConditionsEditHandler : IRequestHandler<AdminTermsConditionsEditCommand, CommandResponse<TermsConditions>>
    {
        private readonly ITermsConditionsRepository _repository;
        public AdminTermsConditionsEditHandler(ITermsConditionsRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<TermsConditions>> Handle(AdminTermsConditionsEditCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            entity.Content = request.Content;
            entity.Enable = request.Enable;
            entity.Type = request.Type;

            await _repository.UpdateAsync(entity);
            return new Success<TermsConditions>() { Data = entity };
        }
    }
}

