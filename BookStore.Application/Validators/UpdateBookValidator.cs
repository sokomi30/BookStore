using BookStore.Application.DTOs;
using FluentValidation;

namespace BookStore.Application.Validators;

public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookValidator()
    {
        RuleFor(x => x.ISBN)
            .Length(10, 13).WithMessage("ISBN должен быть от 10 до 13 символов")
            .When(x => !string.IsNullOrEmpty(x.ISBN));

        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0")
            .When(x => x.Price.HasValue && x.Price > 0);
    }
}
