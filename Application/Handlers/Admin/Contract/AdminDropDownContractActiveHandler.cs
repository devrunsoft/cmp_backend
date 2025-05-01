using System;
using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application.Handlers.Admin
{
    public class AdminDropDownContractActiveHandler : IRequestHandler<AdminDropDownContractActiveCommand, CommandResponse<List<NameAndValue<Contract>>>>
    {
        private readonly IContractRepository _repository;
        public AdminDropDownContractActiveHandler(IContractRepository reposiotry)
        {
            _repository = reposiotry;
        }
        public async Task<CommandResponse<List<NameAndValue<Contract>>>> Handle(AdminDropDownContractActiveCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p=>p.Active)).Select(x=> new NameAndValue<Contract>() { name = x.Title , value = x}).ToList();

            return new Success<List<NameAndValue<Contract>>> () { Data = result };
        }
    }
}

