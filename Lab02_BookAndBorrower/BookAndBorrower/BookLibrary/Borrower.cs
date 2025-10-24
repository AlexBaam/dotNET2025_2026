namespace Lab_02.BookLibrary;

/**
 * Borrower record defines a library member who borrows books.
 *
 * @string ID - The unique identifier of the borrower.
 * @string Name - The full name of the borrower.
 * @List<Book> BorrowedBooks - A list of books currently borrowed by the member.
 */
public record Borrower(string ID, string Name, List<Book> BorrowedBooks)
{
    /**
     * Secondary constructor allowing creation of a borrower without
     * specifying a borrowed book list (creates an empty list by default).
     *
     * @param ID - The unique identifier for the borrower.
     * @param Name - The full name of the borrower.
     */
    public Borrower(string ID, string Name)
        : this(ID, Name, new List<Book>()) { }
}