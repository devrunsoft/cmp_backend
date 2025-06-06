using System;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using CMPNatural.Core.Entities;
using System.Linq;
using CMPNatural.Application.Commands;
using CMPNatural.Application.Model;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CMPNatural.Application
{
    public class InformationHandler : IRequestHandler<InformationCommand, CommandResponse<CommonInformationModel>>
    {
        private readonly IAppInformationRepository _repository;
        public InformationHandler(IAppInformationRepository providerReposiotry)
        {
            _repository = providerReposiotry;
        }
        public async Task<CommandResponse<CommonInformationModel>> Handle(InformationCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _repository.GetAllAsync()).FirstOrDefault();

            return new Success<CommonInformationModel>() { Data = CommonAppInformationMapper.Mapper.Map<CommonInformationModel>(entity) };
        }
    }
}

