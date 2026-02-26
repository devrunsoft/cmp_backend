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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CMPNatural.Application
{
    public class AdminGetPaginateClientSessionsHandler : IRequestHandler<AdminGetPaginateClientSessionsCommand, CommandResponse<List<ChatClientSession>>>
    {
        private readonly IChatClientSessionRepository _repository;

        public AdminGetPaginateClientSessionsHandler(IChatClientSessionRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<List<ChatClientSession>>> Handle(AdminGetPaginateClientSessionsCommand request, CancellationToken cancellationToken)
        {
            var query = (await _repository.GetAsync(p => p.ChatSession.Any(x => x.Messages.Any()),
                query => query
                .Include(x => x.Company)
                .Include(x => x.ChatSession)
                .ThenInclude(x => x.OperationalAddress)
                .Include(x => x.ChatSession)
                .ThenInclude(x => x.Messages)
                ));

            var companyFilter = QueryCompanyExtensions.FilterByCompanyQuery(request.allField);

            var filter = ExpressionExtensions.ApplyToNavigation<ChatClientSession, Company>(
                companyFilter,
                p => p.Company
            );

            var result = query
                .AsQueryable()
                .Where(filter)
                .OrderByDescending(x =>
                    (x.ChatSession ?? new List<ChatSession>())
                        .Where(s => s != null)
                        .SelectMany(s => s.Messages ?? new List<ChatMessage>())
                        .Select(m => (long?)m.Id)
                        .Max() ?? 0)
                .ToList();

            foreach (var item in result)
            {
                foreach (var i in item.ChatSession ?? new List<ChatSession>())
                {
                    i.UnRead = (i.Messages ?? new List<ChatMessage>())
                        .Count(m => !m.IsSeen && m.SenderType != SenderType.Admin);
                }
            }

            return new Success<List<ChatClientSession>>() { Data = result };
        }
    }
}
