namespace BookManagement.Features.Books;
/*
 * Request DTO for deleting a book by its unique identifier.
 * @param Id - Unique identifier of the book to delete.
 */
public record DeleteBookRequest(Guid Id);