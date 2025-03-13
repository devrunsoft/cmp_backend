using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Base;
using Microsoft.EntityFrameworkCore;

namespace CMPNatural.Application
{
    public class AdminGetAllHandler : IRequestHandler<AdminGetAllCommand, CommandResponse<PagesQueryResponse<AdminEntity>>>
    {
        private readonly IAdminRepository _reposiotry;

        public AdminGetAllHandler(IAdminRepository reposiotry)
        {
            _reposiotry = reposiotry;
        }

        public async Task<CommandResponse<PagesQueryResponse<AdminEntity>>> Handle(AdminGetAllCommand request, CancellationToken cancellationToken)
        {
            var result = (await _reposiotry.GetBasePagedAsync(request,x=>x.Role != "SuperAdmin", query =>query.Include(x=>x.Person)));
            return new Success<PagesQueryResponse<AdminEntity>>() { Data = result };
        }
    }
}

