using FluentValidation;
using BookManagement.Features.Books;

namespace BookManagement.Validators;

public class UpdateBookValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookValidator()
    {
        RuleFor(b => b.Title)
            .Must(t => t == null || !string.IsNullOrWhiteSpace(t))
            .WithMessage("Title cannot be empty if provided.")
            .MaximumLength(200).When(b => b.Title != null);

        RuleFor(b => b.Author)
            .Must(a => a == null || !string.IsNullOrWhiteSpace(a))
            .WithMessage("Author cannot be empty if provided.")
            .MaximumLength(100).When(b => b.Author != null);

        RuleFor(b => b.Year)
            .Must(y => !y.HasValue || y.Value > 0)
            .WithMessage("Year must be greater than 0 if provided.");
    }
}