//using System;
//using CMPNatural.Core.Base;
//using CMPNatural.Core.Repositories;
//using MediatR;
//using ScoutDirect.Application.Responses;
//using System.Threading;
//using System.Threading.Tasks;
//using CMPNatural.Core.Entities;
//using CMPNatural.Application.Commands;
//using System.Linq;
//using System.Collections.Generic;

//namespace CMPNatural.Application
//{
//    public class GetAllCompanyContractHandler : IRequestHandler<GetAllCompanyContractCommand, CommandResponse<List<CompanyContract>>>
//    {
//        private readonly ICompanyContractRepository _repository;
//        public GetAllCompanyContractHandler(ICompanyContractRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<CommandResponse<List<CompanyContract>>> Handle(GetAllCompanyContractCommand request, CancellationToken cancellationToken)
//        {
//            var invoices = (await _repository.GetAsync(p =>  p.CompanyId == request.CompanyId)).OrderBy(x => x.Status).ToList();
//            return new Success<List<CompanyContract>>() { Data = invoices };
//        }
//    }
//}

