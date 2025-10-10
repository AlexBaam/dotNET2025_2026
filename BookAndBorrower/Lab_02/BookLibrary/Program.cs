namespace Lab_02.BookLibrary;

public class Program
{
    static void Main()
    {
        
        var library = new Library();
        
        var fiction = new LibrarySection("Fiction");
        var librarian = new Librarian("Alice", "alice@library.com", fiction);
        library.AddSection(fiction);
        library.AddLibrarian(librarian);
        
        fiction.AddBook(new Book("C# in Depth", "Jon Skeet", 2021));
        fiction.AddBook(new Book("Clean Code", "Robert C. Martin", 2008));
        fiction.AddBook(new Book("The Pragmatic Programmer", "Andy Hunt", 2019));
        
        Console.WriteLine("Enter a new book title and author:");
        string? title = Console.ReadLine();
        string? author = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author))
        {
            fiction.AddBook(new Book(title, author, 2025));
        }
        else if (!string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(author))
        {
            fiction.AddBook(new Book(title, "Unknown Author", 2025));
        }
        
        Console.WriteLine("\nAll books in Fiction section:");
        foreach (var book in fiction.ListBooks())
            Console.WriteLine($"- {book.Title} ({book.YearPublished})");

        Console.WriteLine("\nBooks published after 2010:");
        var recentBooks = fiction.ListBooks().Where(static b => b.YearPublished > 2010);
        foreach (var book in recentBooks)
            Console.WriteLine($"- {book.Title} ({book.YearPublished})");
        
        var borrower1 = new Borrower("B001", "John Doe");
        var borrowedBook = new Book("C# in Depth", "Jon Skeet", 2021);
        var updatedBorrower = borrower1 with
        {
            BorrowedBooks = borrower1.BorrowedBooks.Append(borrowedBook).ToList()
        };
        
        Console.WriteLine("\n--- Pattern Matching Output ---");
        Display.DisplayInfo(borrowedBook);
        Display.DisplayInfo(updatedBorrower);
        Display.DisplayInfo("Random string");

    }
}