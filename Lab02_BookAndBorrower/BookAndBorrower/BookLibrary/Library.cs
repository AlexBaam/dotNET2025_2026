namespace Lab_02.BookLibrary;

/**
 * Library class represents a collection of sections, librarians, and borrowers.
 * It allows adding and retrieving these entities.
 *
 * @List<LibrarySection> sections - Contains all the sections in the library.
 * @List<Librarian> librarians - Contains all librarians working in the library.
 * @List<Borrower> borrowers - Contains all borrowers registered with the library.
 */
public class Library
{
    // A list holding all library sections
    private readonly List<LibrarySection> sections = new List<LibrarySection>();

    // A list holding all librarians working in the library
    private readonly List<Librarian> librarians = new List<Librarian>();

    // A list holding all borrowers who can borrow books
    private readonly List<Borrower> borrowers = new List<Borrower>();

    /**
     * Adds a new section to the library.
     * @param section - The section object to be added.
     */
    public void AddSection(LibrarySection section) => sections.Add(section);

    /**
     * Adds a new librarian to the library.
     * @param librarian - The librarian object to be added.
     */
    public void AddLibrarian(Librarian librarian) => librarians.Add(librarian);

    /**
     * Adds a new borrower to the library.
     * @param borrower - The borrower object to be added.
     */
    public void AddBorrower(Borrower borrower) => borrowers.Add(borrower);

    /**
     * Retrieves a list of all sections in the library.
     * @return IEnumerable<LibrarySection> - Collection of library sections.
     */
    public IEnumerable<LibrarySection> ListSections => sections;

    /**
     * Retrieves a list of all librarians in the library.
     * @return IEnumerable<Librarian> - Collection of librarians.
     */
    public IEnumerable<Librarian> ListLibrarians => librarians;

    /**
     * Retrieves a list of all borrowers in the library.
     * @return IEnumerable<Borrower> - Collection of borrowers.
     */
    public IEnumerable<Borrower> ListBorrowers => borrowers;
}