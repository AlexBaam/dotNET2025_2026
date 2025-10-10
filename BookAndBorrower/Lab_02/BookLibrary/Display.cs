namespace Lab_02.BookLibrary;

/**
 * Display static class provides a method to output information
 * about different object types using pattern matching.
 */
public static class Display
{
    /**
     * Displays object information based on its type.
     * - If the object is a Book, it displays the title and publication year.
     * - If the object is a Borrower, it displays the borrower's name and number of borrowed books.
     * - Otherwise, it displays "Unknown type".
     *
     * @param obj - The object to display information for.
     */
    public static void DisplayInfo(object obj)
    {
        switch (obj)
        {
            case Book b:
                Console.WriteLine($"Book: {b.Title} ({b.YearPublished})");
                break;
            case Borrower br:
                Console.WriteLine($"Borrower: {br.Name}, Borrowed Books: {br.BorrowedBooks.Count}");
                break;
            default:
                Console.WriteLine("Unknown type");
                break;
        }
    }
}