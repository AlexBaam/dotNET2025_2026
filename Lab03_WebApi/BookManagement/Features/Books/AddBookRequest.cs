namespace BookManagement.Features.Books;
/*
 * Request DTO for creating a new book.
 * @param Title - Title of the book.
 * @param Author - Author of the book.
 * @param Year - Year of publication.
 */
public record AddBookRequest(string Title, string Author, int Year);