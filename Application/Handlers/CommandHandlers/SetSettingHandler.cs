using Bazaro.Application.Commands;
using Bazaro.Application.Responses.Base;
using Bazaro.Application.Validator;
using Bazaro.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Bazaro.Application.Handlers.CommandHandlers
{
    public class SetSettingHandler : IRequestHandler<SetSettingCommand, CommandResponse>
    {
        private readonly IShopRepository _shopRepository;
        //private readonly IShopInfoRepository _shopInfoRepository;

        public SetSettingHandler(IShopRepository shopRepository/*, IShopInfoRepository shopInfoRepository*/)
        {
            _shopRepository = shopRepository;
            //_shopInfoRepository = shopInfoRepository;
        }

        public async Task<CommandResponse> Handle(SetSettingCommand request, CancellationToken cancellationToken)
        {
            var results = new SetSettingCommandValidator().Validate(request);

            if (!results.IsValid)
                return new HasError(results);

            // var dbModel = await _shopInfoRepository.GetByIdAsync(request.Id);

            // if (dbModel == null)
            //     return new ResponseNotFound();

            // dbModel.DeliveryId = request.DeliveriesModelId;
            // dbModel.OrderMinPriceId = request.OrderMinPriceId;
            // await _shopInfoRepository.UpdateAsync(dbModel);


            //if (!string.IsNullOrWhiteSpace(dbModel.Name))
            //    dbModel.Name = dbModel.Name;


            var dbShopModel = await _shopRepository.GetByIdAsync(request.Id);

            if (dbShopModel == null)
                return new ResponseNotFound();

            dbShopModel.IsEnable = request.IsEnable;

            await _shopRepository.UpdateAsync(dbShopModel);
            return new Success() { };
        }
    }

}
