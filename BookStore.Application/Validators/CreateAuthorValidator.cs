using FluentValidation;
using BookStore.Application.DTOs;

namespace BookStore.Application.Validators
{
    public class CreateAuthorValidator : AbstractValidator<CreateAuthorDto>
    {
        public CreateAuthorValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Имя автора обязательно")
                .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Дата рождения обязательна")
                .LessThan(DateTime.UtcNow).WithMessage("Дата рождения не может быть в будущем")
                .GreaterThan(new DateTime(1000, 1, 1)).WithMessage("Некорректная дата рождения");
        }
    }
}