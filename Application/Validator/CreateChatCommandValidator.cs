using Bazaro.Application.Commands;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
    {
        public CreateChatCommandValidator()
        {
            RuleFor(customer => customer.InboxId).NotNull();
        }
    }
}
