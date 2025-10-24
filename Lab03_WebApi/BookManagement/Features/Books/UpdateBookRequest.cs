namespace BookManagement.Features.Books;
/*
 * Request DTO for updating a book's details.
 * @param Id - Unique identifier of the book to update.
 * @param Title - Optional new title.
 * @param Author - Optional new author.
 * @param Year - Optional new year of publication.
 */
public class UpdateBookRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? Year { get; set; }
}