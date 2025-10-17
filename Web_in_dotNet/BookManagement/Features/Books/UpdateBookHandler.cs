using BookManagement.Persistence;
using BookManagement.Validators;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Features.Books;

public class UpdateBookHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;

    public async Task<IResult> Handle(UpdateBookRequest request)
    {
        var validator = new UpdateBookValidator();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        { 
            return Results.BadRequest(validationResult.Errors);
        }
        
        var book = await _context.Books.FindAsync(request.Id);
        if (book == null)
        {
            return Results.NotFound();
        }

        var updatedBook = book with
        {
            Title = request.Title ?? book.Title,
            Author = request.Author ?? book.Author,
            Year = request.Year ?? book.Year
        };

        _context.Entry(book).CurrentValues.SetValues(updatedBook);
        await _context.SaveChangesAsync();

        return Results.Ok(updatedBook);
    }
}