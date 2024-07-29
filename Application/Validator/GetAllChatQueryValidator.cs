using Bazaro.Application.Queries;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class GetAllChatQueryValidator : AbstractValidator<GetAllChatQuery>
    {
        public GetAllChatQueryValidator()
        {
            RuleFor(query => query.Page).NotEmpty().WithMessage("لطفا شماره صفحه را وارد نمایید");
            RuleFor(query => query.Size).NotEmpty().WithMessage("لطفا اندازه هر صفحه را وارد نمایید");
            RuleFor(query => query.ShopId).NotEmpty().WithMessage("لطفا شناسه فروشگاه را وارد نمایید");
        }
    }
}
