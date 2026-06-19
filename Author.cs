namespace BookLibrary.Entities;

/// <summary>
/// Represents a book author.
/// </summary>
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property — one author has many books
    public ICollection<Book> Books { get; set; } = new List<Book>();

    public override string ToString() => $"[Author #{Id}] {Name}";
}
