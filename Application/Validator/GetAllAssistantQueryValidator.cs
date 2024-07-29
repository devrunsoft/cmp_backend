using Bazaro.Application.Queries;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class GetAllAssistantQueryValidator : AbstractValidator<GetAllAssistantQuery>
    {
        public GetAllAssistantQueryValidator()
        {
            RuleFor(query => query.ShopId).NotEmpty().WithMessage("لطفا شناسه فروشگاه را وارد نمایید");
        }
    }
}
