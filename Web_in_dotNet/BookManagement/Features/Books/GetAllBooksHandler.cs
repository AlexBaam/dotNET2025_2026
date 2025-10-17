using BookManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Features.Books;

public class GetAllBooksHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;
    
    public async Task<IResult> Handle(GetAllBooksRequest request)
    {
        var descending = request.Descending ?? false;
        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? 10;
        
        var query = _context.Books.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.Author))
        {
            query = query.Where(b => b.Author.Contains(request.Author));
        }
        
        query = request.SortBy?.ToLower() switch
        {
            "title" => descending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
            "year"  => descending ? query.OrderByDescending(b => b.Year)  : query.OrderBy(b => b.Year),
            _       => query.OrderBy(b => b.Id)
        };
        
        var skip = (pageNumber - 1) * pageSize;
        var books = await query.Skip(skip).Take(pageSize).ToListAsync();
        
        return Results.Ok(books);
    }
}