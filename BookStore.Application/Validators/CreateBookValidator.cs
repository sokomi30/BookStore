using BookStore.Application.DTOs;
using FluentValidation;

namespace BookStore.Application.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN обязателен")
            .Length(10, 13).WithMessage("ISBN должен быть от 10 до 13 символов");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название обязательно")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена должна быть больше 0");

        RuleFor(x => x.AuthorId)
            .GreaterThan(0).WithMessage("Должен быть указан существующий автор");
    }
}