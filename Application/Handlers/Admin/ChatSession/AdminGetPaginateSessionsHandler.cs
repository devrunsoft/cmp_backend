using CMPNatural.Core.Base;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories.Chat;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application
{
    public class AdminGetPaginateSessionsHandler : IRequestHandler<AdminGetPaginateSessionsCommand, CommandResponse<List<ChatSession>>>
    {
        private readonly IChatSessionRepository _repository;

        public AdminGetPaginateSessionsHandler(IChatSessionRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<List<ChatSession>>> Handle(AdminGetPaginateSessionsCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.ClientId == request.ClientId,
                query => query
                .Include(x => x.Company)
                .Include(x => x.OperationalAddress)
                )).ToList();
            return new Success<List<ChatSession>>() { Data = result };
        }
    }
}