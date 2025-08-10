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
    public class AdminGetChatSessionHandler : IRequestHandler<AdminGetChatSessionCommand, CommandResponse<ChatSession>>
    {
        private readonly IChatSessionRepository _repository;

        public AdminGetChatSessionHandler(IChatSessionRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<CommandResponse<ChatSession>> Handle(AdminGetChatSessionCommand request, CancellationToken cancellationToken)
        {
            var result = (await _repository.GetAsync(p => p.OperationalAddressId == request.OperationalAddressId,
                 query => query
                .Include(x => x.Company)
                .Include(x => x.OperationalAddress)
                )).FirstOrDefault();
            return new Success<ChatSession>() { Data = result };
        }
    }
}

