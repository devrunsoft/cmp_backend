using Bazaro.Application.Queries;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class GetCustomerProfileQueryValidator : AbstractValidator<GetCustomerProfileQuery>
    {
        public GetCustomerProfileQueryValidator()
        {
            RuleFor(query => query.Id).NotEmpty().WithMessage("لطفا شناسه مشتری را وارد نمایید");
        }
    }
}
