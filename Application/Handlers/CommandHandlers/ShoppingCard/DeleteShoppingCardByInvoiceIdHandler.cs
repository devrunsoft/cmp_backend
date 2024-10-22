using System;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Repositories;
using MediatR;
using ScoutDirect.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CMPNatural.Application.Commands.ShoppingCard;

namespace CMPNatural.Application.Handlers
{

    public class DeleteShoppingCardByInvoiceIdHandler : IRequestHandler<DeleteAllShoppingCardCommand, CommandResponse<object>>
    {
        private readonly IShoppingCardRepository _shoppingCardRepository;

        public DeleteShoppingCardByInvoiceIdHandler(IShoppingCardRepository shoppingCardRepository)
        {
            _shoppingCardRepository = shoppingCardRepository;
        }

        public async Task<CommandResponse<object>> Handle(DeleteAllShoppingCardCommand request, CancellationToken cancellationToken)
        {

            var entity = (await _shoppingCardRepository.GetAsync(p => p.CompanyId == request.CompanyId)).ToList();

            if (entity == null)
            {
                return new NoAcess<object>() { };
            }
            await _shoppingCardRepository.DeleteRangeAsync(entity);

            return new Success<object>() { };
        }
    }
}

