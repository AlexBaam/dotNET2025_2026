public record Book(string Title, string Author)
{
    public bool IsBorrowed { get; set; }
    public User? BorrowedBy { get; set; }
}

public record User
{
    public string Name { get; init; }
    public User(string name) => Name = name;
}

public class Library
{
    private readonly List<Book> books = new List<Book>();
    private readonly List<User> users = new List<User>();
    
    public void AddBook(Book book) => books.Add(book);
    public void AddUser(User user) => users.Add(user);
    
    public IEnumerable<Book> ListBooks() => books;
    public IEnumerable<User> ListUsers() => users;

    public bool BorrowBook(string title, string userName)
    {
        var book = books.FirstOrDefault(b => b.Title == title && !b.IsBorrowed);
        var user = users.FirstOrDefault(u => u.Name == userName);

        if (book is null | user is null)
        {
            return false;
        }
        
        book.IsBorrowed = true;
        book.BorrowedBy = user;
        return true;
    }
    
    public bool ReturnBook(string title)
    {
        var book = books.FirstOrDefault(b => b.Title == title && b.IsBorrowed);
        if (book is null)
        {
            return false;
        }
        
        book.IsBorrowed = false;
        book.BorrowedBy = null;
        return true;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var library = new Library();
    
        library.AddBook(new Book("1984", "George Orwell"));
        library.AddBook(new Book("To Kill a Mockingbird", "Harper Lee"));
        library.AddUser(new User("Alice"));
        library.AddUser(new User("Bob"));

        while (true)
        {
            Console.WriteLine("\n Virtual Library Menu:");
            Console.WriteLine("1. List all books");
            Console.WriteLine("2. Add new book");
            Console.WriteLine("3. List all users");
            Console.WriteLine("4. Add new user");
            Console.WriteLine("5. Borrow a book");
            Console.WriteLine("6. Return a book");
            Console.WriteLine("0. Exit");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("\nBooks in Library:");
                    foreach (var book in library.ListBooks())
                    {
                        Console.WriteLine($"- {book.Title} by {book.Author}");
                    }

                    break;
                case "2":
                    Console.Write("Enter book title: ");
                    var title = Console.ReadLine();
                    Console.Write("Enter book author: ");
                    var author = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author))
                    {
                        library.AddBook(new Book(title, author));
                        Console.WriteLine("Book added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Book not added.");
                    }

                    break;
                case "3":
                    Console.WriteLine("\nRegistered Users:");
                    foreach (var user in library.ListUsers())
                    {
                        Console.WriteLine($"- {user.Name}");
                    }
                    
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
    }
}