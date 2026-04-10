using System;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CMPNatural.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CMPNatural.Core.Repositories.ChatCommon;
using CMPNatural.Core.Enums;

namespace CMPNatural.Application
{
    public class CreateChatCommonSessionHandler : IRequestHandler<CreateChatCommonSessionCommand, CommandResponse<ChatCommonSession>>
    {
        private readonly IChatCommonSessionRepository _repository;
        private readonly IDriverRepository driverRepository;
        private readonly IProviderReposiotry providerReposiotry;
        public CreateChatCommonSessionHandler(IChatCommonSessionRepository _repository, IDriverRepository driverRepository, IProviderReposiotry providerReposiotry)
        {
            this._repository = _repository;
            this.driverRepository = driverRepository;
            this.providerReposiotry = providerReposiotry;

        }
        public async Task<CommandResponse<ChatCommonSession>> Handle(CreateChatCommonSessionCommand request, CancellationToken cancellationToken)
        {
            var chatsession = (await _repository.GetAsync(x => x.ParticipantId == request.ParticipantId && x.IsActive
                )).LastOrDefault();
            Guid personId;

            if(request.ParticipantType== ParticipantType.Provider)
            {
                var provider = (await providerReposiotry.GetAsync(x => x.Id == request.ParticipantId)).FirstOrDefault();
                personId = provider.PersonId.Value;
            }
            else
            {
                var provider = (await driverRepository.GetAsync(x => x.Id == request.ParticipantId)).FirstOrDefault();
                personId = provider.PersonId;
            }


            if (chatsession == null)
            {
                var entity = new ChatCommonSession
                {
                    ParticipantId = request.ParticipantId,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    ParticipantType = request.ParticipantType,
                    PersonId = personId,
                };
                var result = await _repository.AddAsync(entity);
                return new Success<ChatCommonSession>() { Data = result };
            }

            return new Success<ChatCommonSession>() { Data = chatsession };

        }
    }
}

