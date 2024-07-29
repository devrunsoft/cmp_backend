using Bazaro.Application.Commands;
using FluentValidation;

namespace Bazaro.Application.Validator
{
    public class SetSettingCommandValidator : AbstractValidator<SetSettingCommand>
    {
        public SetSettingCommandValidator()
        {
            //RuleFor(setting => setting.DeliveriesModelId).NotNull();
        }
    }
}
