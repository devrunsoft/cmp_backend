using System;
using CMPNatural.Application.Commands.Billing;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Application.Commands.Document;
using System.Collections.Generic;
using CMPNatural.Application.Commands;

namespace CMPNatural.Application.Handlers.CommandHandlers.Document
{
    public class BusinessTypeHandler : IRequestHandler<GetAllBusinessTypeCommand, CommandResponse<List<BusinessType>>>
    {
        private readonly IBusinessTypeRepository _businessTypeRepository;

        public BusinessTypeHandler(IBusinessTypeRepository businessTypeRepository)
        {
            _businessTypeRepository = businessTypeRepository;
        }

        public async Task<CommandResponse<List<BusinessType>>> Handle(GetAllBusinessTypeCommand request, CancellationToken cancellationToken)
        {
            var result = (await _businessTypeRepository.GetAllAsync()).ToList();

            return new Success<List<BusinessType>>() { Data = result };
        }

    }
}

