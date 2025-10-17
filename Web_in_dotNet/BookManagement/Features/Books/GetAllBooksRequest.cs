namespace BookManagement.Features.Books;
/*
 * Request DTO for retrieving books with optional filtering, sorting, and pagination.
 * @param Author - Optional author name to filter by.
 * @param SortBy - Optional property to sort by ("title" or "year").
 * @param Descending - Whether sorting should be descending.
 * @param PageNumber - Page number for pagination (default 1).
 * @param PageSize - Number of items per page (default 10).
 */
public class GetAllBooksRequest
{
    public string? Author { get; set; }
    public string? SortBy { get; set; }
    public bool? Descending { get; set; } = false;
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}