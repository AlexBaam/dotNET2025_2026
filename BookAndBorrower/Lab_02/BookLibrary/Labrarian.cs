namespace Lab_02.BookLibrary;

/**
 * Librarian class represents a staff member responsible for managing
 * a specific section within the library.
 *
 * @string Name - The full name of the librarian.
 * @string Email - The email address of the librarian.
 * @LibrarySection Section - The library section assigned to this librarian.
 */
public class Librarian
{
    // The full name of the librarian
    private string Name { get; init; }

    // The email address of the librarian
    private string Email { get; init; }

    // The section this librarian manages
    private LibrarySection Section { get; init; }

    /**
     * Constructor to initialize a new Librarian with the provided details.
     *
     * @param Name - The full name of the librarian.
     * @param Email - The email address of the librarian.
     * @param Section - The library section assigned to this librarian.
     */
    public Librarian(string Name, string Email, LibrarySection Section)
    {
        this.Name = Name;
        this.Email = Email;
        this.Section = Section;
    }

    /**
     * Returns a formatted string containing librarian details.
     *
     * @return string - A descriptive string with name, email, and section info.
     */
    public override string ToString()
    {
        return $"Librarian: {Name}, Email: {Email}, Section: {Section}";
    }
}