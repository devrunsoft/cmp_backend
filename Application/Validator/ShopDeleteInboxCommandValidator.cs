using Bazaro.Application.Commands;
using Bazaro.Core.Entities;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class ShopDeleteInboxCommandValidator : AbstractValidator<ShopDeleteInboxCommand>
    {
        public ShopDeleteInboxCommandValidator(Inbox dbModel)
        {
            RuleFor(query => query).Custom((query, context) => {

                if ((query.ShopId != null && query.ShopId !=  dbModel.ShopId) || (query.ShopId == null && query.CustomerId != dbModel.CustomerId))
                {
                    context.AddFailure("رکورد مورد نظر از دسترس خارج است");
                }
            });
        }
    }
}
