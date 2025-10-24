namespace Lab_02.BookLibrary;

/**
 * Main program class for the Library Book and Borrower Tracker.
 * Demonstrates:
 * - Creating books, sections, librarians, and borrowers
 * - Using top-level interactions like user input
 * - Pattern matching and static lambda filtering
 * - Cloning records using 'with' expression
 */
public class Program
{
    static void Main()
    {
        // Create a new library
        var library = new Library();

        // Create a new section and a librarian who manages it
        var fiction = new LibrarySection("Fiction");
        var librarian = new Librarian("Alice", "alice@library.com", fiction);

        // Add section and librarian to the library
        library.AddSection(fiction);
        library.AddLibrarian(librarian);

        // Add a few sample books to the Fiction section
        fiction.AddBook(new Book("C# in Depth", "Jon Skeet", 2021));
        fiction.AddBook(new Book("Clean Code", "Robert C. Martin", 2008));
        fiction.AddBook(new Book("The Pragmatic Programmer", "Andy Hunt", 2019));

        // Allow the user to enter a new book title and author
        Console.WriteLine("Enter a new book title and author:");
        string? title = Console.ReadLine();
        string? author = Console.ReadLine();

        // Add the user's book (with author default if empty)
        if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author))
        {
            fiction.AddBook(new Book(title, author, 2025));
        }
        else if (!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
        {
            fiction.AddBook(new Book(title, "Unknown Author", 2025));
        }

        // Display all books in the Fiction section
        Console.WriteLine("\nAll books in Fiction section:");
        foreach (var book in fiction.ListBooks())
            Console.WriteLine($"- {book.Title} ({book.YearPublished})");

        // Use static lambda to filter books published after 2010
        Console.WriteLine("\nBooks published after 2010:");
        var recentBooks = fiction.ListBooks().Where(static b => b.YearPublished > 2010);
        foreach (var book in recentBooks)
            Console.WriteLine($"- {book.Title} ({book.YearPublished})");

        // Create a borrower and clone them with an additional borrowed book using 'with'
        var borrower1 = new Borrower("B001", "John Doe");
        var borrowedBook = new Book("C# in Depth", "Jon Skeet", 2021);
        var updatedBorrower = borrower1 with
        {
            BorrowedBooks = borrower1.BorrowedBooks.Append(borrowedBook).ToList()
        };

        // Demonstrate pattern matching on different object types
        Console.WriteLine("\n--- Pattern Matching Output ---");
        Display.DisplayInfo(borrowedBook);      // Book type
        Display.DisplayInfo(updatedBorrower);   // Borrower type
        Display.DisplayInfo("Random string");   // Unknown type
    }
}
