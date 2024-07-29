using Bazaro.Application.Queries;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class GetCustomerAddressQueryValidator : AbstractValidator<GetCustomerAddressQuery>
    {
        public GetCustomerAddressQueryValidator()
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
