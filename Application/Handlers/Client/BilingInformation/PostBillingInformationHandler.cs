using Barbara.Core.Entities;
using Barbara.Application.Responses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ScoutDirect.Core.Repositories;
using ScoutDirect.Core.Entities;
using ScoutDirect.Application.Responses;

using CMPNatural.Application.Responses;
using CMPNatural.Core.Entities;
using CMPNatural.Application.Mapper;
using CMPNatural.Application.Commands;
using CMPNatural.Core.Repositories;
using System.Linq;

namespace CMPNatural.Application.Handlers.CommandHandlers
{
    public class PostBillingInformationHandler : IRequestHandler<PostBillingInformationCommand, CommandResponse<BillingInformation>>
    {
        private readonly IBillingInformationRepository _billingInformationRepository;

        public PostBillingInformationHandler(IBillingInformationRepository billingInformationRepository)
        {
            _billingInformationRepository = billingInformationRepository;
        }

        public async Task<CommandResponse<BillingInformation>> Handle(PostBillingInformationCommand request, CancellationToken cancellationToken)
        {
                if (request.Id == null)
                {
                    var entity = new BillingInformation()
                    {
                        Address = request.Address,
                        CardholderName = request.CardholderName,
                        ZIPCode = request.ZIPCode,
                        State = request.State,
                        IsPaypal = request.IsPaypal,
                        Expiry = request.Expiry,
                        CVC = request.CVC,
                        CardNumber = request.CardNumber,
                        City = request.City,
                        CompanyId = request.CompanyId

                    };

                    var result = await _billingInformationRepository.AddAsync(entity);

                }
                else
                {
                    var entity = (await _billingInformationRepository.GetAsync(p => p.Id == request.Id)).FirstOrDefault();
                    entity.Address = request.Address;
                    entity.CardholderName = request.CardholderName;
                    entity.ZIPCode = request.ZIPCode;
                    entity.State = request.State;
                    entity.IsPaypal = request.IsPaypal;
                    entity.Expiry = request.Expiry;
                    entity.CVC = request.CVC;
                    entity.CardNumber = request.CardNumber;
                    entity.City = request.City;
                    entity.CompanyId = request.CompanyId;
                    await _billingInformationRepository.UpdateAsync(entity);
                }

            return new Success<BillingInformation>();

        }
    }
}
