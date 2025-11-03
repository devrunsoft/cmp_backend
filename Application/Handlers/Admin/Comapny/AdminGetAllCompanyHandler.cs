using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ScoutDirect.Core.Authentication;
using System.Linq;
using System.Collections.Generic;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Base;

namespace CMPNatural.Application.Handlers.Admin.Auth
{

    public class AdminGetAllCompanyHandler : IRequestHandler<AdminGetAllCompanyCommand, CommandResponse<PagesQueryResponse<CompanyResponse>>>
    {
        private readonly ICompanyRepository _repository;

        public AdminGetAllCompanyHandler(ICompanyRepository invoiceRepository)
        {
            _repository = invoiceRepository;
        }

        public async Task<CommandResponse<PagesQueryResponse<CompanyResponse>>> Handle(AdminGetAllCompanyCommand request, CancellationToken cancellationToken)
        {

            var addressFilter = QueryCompanyExtensions.FilterByQuery(request.allField, request.CompanyStatus);
            var command = new PagedQueryRequest() { Page = request.Page , Size = request.Size };

            var invoices = (await _repository.GetBasePagedAsync(command, addressFilter,
                query => query.Include(x=>x.OperationalAddress).Include(x=>x.CompanyContract)
                ));

            var model = new PagesQueryResponse<CompanyResponse>(
                invoices.elements.Select(p => CompanyMapper.Mapper.Map<CompanyResponse>(p)).ToList(),
                invoices.pageNumber,
                invoices.totalPages,
                invoices.totalElements);

            return new Success<PagesQueryResponse<CompanyResponse>>() { Data = model };

        }

    }
}

