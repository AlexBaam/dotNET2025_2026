namespace Lab_02.BookLibrary;

/**
 * Book record to define the books we can find in a library.
 *  @string Title - The title of the book.
 *  @string Author - The author of the book.
 *  @int YearPublished - The year the book was published.
 */
public record Book(string Title, string Author, int YearPublished);