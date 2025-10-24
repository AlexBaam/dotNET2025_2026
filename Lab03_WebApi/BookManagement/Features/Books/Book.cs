namespace BookManagement.Features.Books;
/*
 * Represents a book entity with basic details.
 * @param Id - Unique identifier.
 * @param Title - Book title.
 * @param Author - Book author.
 * @param Year - Year of publication.
 */
public record Book(Guid Id, string Title, string Author, int Year);