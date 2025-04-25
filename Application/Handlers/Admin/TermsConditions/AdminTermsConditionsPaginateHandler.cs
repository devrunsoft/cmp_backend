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
    public class AdminTermsConditionsPaginateHandler : IRequestHandler<AdminTermsConditionsPaginateCommand, CommandResponse<PagesQueryResponse<TermsConditions>>>
    {
        private readonly ITermsConditionsRepository _repository;

        public AdminTermsConditionsPaginateHandler(ITermsConditionsRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }

        public async Task<CommandResponse<PagesQueryResponse<TermsConditions>>> Handle(AdminTermsConditionsPaginateCommand request, CancellationToken cancellationToken)
        {
            var invoices = (await _repository.GetBasePagedAsync(request, p => request.Enable == null || p.Enable == request.Enable, null));

            return new Success<PagesQueryResponse<TermsConditions>>() { Data = invoices };
        }
    }
}

