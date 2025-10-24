namespace Lab_02.BookLibrary;

/**
 * LibrarySection class represents a section within the library,
 * such as Fiction, Science, or History.
 *
 * Each section contains a name and a list of books that belong to it.
 *
 * @string SectionName - The name of the library section.
 * @List<Book> SectionBooks - The list of books in this section.
 */
public class LibrarySection
{
    // Name of the library section
    private string SectionName { get; init; }

    // List of books contained in this section
    private List<Book> SectionBooks = new List<Book>();

    /**
     * Constructor to initialize a new library section with a name.
     * @param SectionName - The name assigned to this library section.
     */
    public LibrarySection(string SectionName)
    {
        this.SectionName = SectionName;
        this.SectionBooks = new List<Book>();
    }

    /**
     * Adds a new book to the section.
     * @param book - The book object to add to the section.
     */
    public void AddBook(Book book) => SectionBooks.Add(book);

    /**
     * Retrieves all books in the section.
     * @return IEnumerable<Book> - Collection of books in this section.
     */
    public IEnumerable<Book> ListBooks() => SectionBooks;
}