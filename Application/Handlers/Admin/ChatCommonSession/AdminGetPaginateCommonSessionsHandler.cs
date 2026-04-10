using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses.ChatCommon;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using CMPNatural.Core.Repositories.ChatCommon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminGetPaginateCommonSessionsHandler : IRequestHandler<AdminGetPaginateCommonSessionsCommand, CommandResponse<List<ChatCommonSessionEntity>>>
    {
        private readonly IChatCommonSessionRepository _repository;
        private readonly IDriverRepository _driverRepository;
        private readonly IProviderReposiotry _providerReposiotry;

        public AdminGetPaginateCommonSessionsHandler(
            IChatCommonSessionRepository repository,
            IDriverRepository driverRepository,
            IProviderReposiotry providerReposiotry)
        {
            _repository = repository;
            _driverRepository = driverRepository;
            _providerReposiotry = providerReposiotry;
        }

        public async Task<CommandResponse<List<ChatCommonSessionEntity>>> Handle(AdminGetPaginateCommonSessionsCommand request, CancellationToken cancellationToken)
        {
            var query = await _repository.GetAsync(
                p => true,
                q => q.Include(x => x.Messages));

            var result = query
                .AsQueryable()
                .OrderByDescending(x => x.Messages.Select(m => (long?)m.Id).Max() ?? 0)
                .ToList();

            foreach (var item in result)
            {
                item.UnRead = (item.Messages ?? new List<ChatCommonMessage>())
                    .Count(m => !m.IsSeen && m.SenderType != ParticipantType.Admin);
            }

            var mapped = await ChatCommonSessionMapper.MapListAsync(result, _driverRepository, _providerReposiotry);

            return new Success<List<ChatCommonSessionEntity>> { Data = mapped };
        }
    }
}
