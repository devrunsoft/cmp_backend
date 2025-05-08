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
    public class GetAllLogHandler : IRequestHandler<AdminGetAllLogCommand, CommandResponse<PagesQueryResponse<AppLog>>>
    {
        private readonly IAppLogRepository _repository;

        public GetAllLogHandler(IAppLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PagesQueryResponse<AppLog>>> Handle(AdminGetAllLogCommand request, CancellationToken cancellationToken)
        {
            var models = (await _repository.GetBasePagedAsync(request));
            return new Success<PagesQueryResponse<AppLog>>() { Data = models };
        }
    }
}

