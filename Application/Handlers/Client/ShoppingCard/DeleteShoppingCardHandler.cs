using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CMPNatural.Application.Handlers
{
    public class DeleteShoppingCardHandler : IRequestHandler<DeleteShoppingCardCommand, CommandResponse<ShoppingCard>>
    {
        private readonly IShoppingCardRepository _shoppingCardRepository;

        public DeleteShoppingCardHandler(IShoppingCardRepository shoppingCardRepository)
        {
            _shoppingCardRepository = shoppingCardRepository;
        }

        public async Task<CommandResponse<ShoppingCard>> Handle(DeleteShoppingCardCommand request, CancellationToken cancellationToken)
        {

            ShoppingCard entity = (await _shoppingCardRepository.GetAsync(p => p.CompanyId == request.CompanyId && p.Id == request.Id)).FirstOrDefault();

            if (entity == null)
            {
                return new NoAcess<ShoppingCard>() {  };
            }
            await _shoppingCardRepository.DeleteAsync(entity);

            return new Success<ShoppingCard>() { Data = entity };
        }
    }
}

