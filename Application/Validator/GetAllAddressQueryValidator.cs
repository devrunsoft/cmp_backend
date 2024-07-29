using Bazaro.Application.Queries;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class GetAllAddressQueryValidator : AbstractValidator<GetAllAddressQuery>
    {
        public GetAllAddressQueryValidator()
        {
            RuleFor(query => query).Custom((query, context) => {
                if (query.ShopId == null && query.Id == null)
                {
                    context.AddFailure("لطفا یکی از ورودی ها را مقدار دهی فرمایید");
                }
            });
        }
    }
}
