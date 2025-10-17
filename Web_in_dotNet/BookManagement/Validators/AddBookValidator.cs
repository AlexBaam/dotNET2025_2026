using FluentValidation;
using BookManagement.Features.Books;

namespace BookManagement.Validators;

public class AddBookValidator : AbstractValidator<AddBookRequest>
{
    public AddBookValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

        RuleFor(b => b.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(30).WithMessage("Author cannot exceed 30 characters.");

        RuleFor(b => b.Year)
            .GreaterThan(1950).WithMessage("Year must be greater than 1950.")
            .LessThan(DateTime.Now.Year + 1).WithMessage("Year must be less than next year.");
    }
}