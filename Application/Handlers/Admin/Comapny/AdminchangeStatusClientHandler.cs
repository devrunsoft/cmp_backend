using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Services;
using System.Collections.Generic;
using System.Linq;
using CMPNatural.Application.Commands.Admin.Company;
using ScoutDirect.Core.Repositories;
using CMPNatural.Application.Responses;
using CMPNatural.Application.Mapper;

namespace CMPNatural.Application
{
    public class AdminchangeStatusClientHandler : IRequestHandler<AdminChangeStatusClientCommand, CommandResponse<CompanyResponse>>
    {
        private readonly ICompanyRepository _reposiotry;

        public AdminchangeStatusClientHandler(ICompanyRepository _reposiotry)
        {
            this._reposiotry = _reposiotry;
        }

        public async Task<CommandResponse<CompanyResponse>> Handle(AdminChangeStatusClientCommand request, CancellationToken cancellationToken)
        {
            var entity = await _reposiotry.GetByIdAsync(request.ClientId);

            entity.Status = request.Status;

            await _reposiotry.UpdateAsync(entity);

            return new Success<CompanyResponse>() { Data = CompanyMapper.Mapper.Map<CompanyResponse>(entity) };

        }
    }
}

