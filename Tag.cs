namespace BookLibrary.Entities;

/// <summary>
/// Represents a genre / category tag that can be applied to books.
/// </summary>
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property → many-to-many Books via BookTag
    public ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();

    public override string ToString() => $"[Tag #{Id}] {Name}";
}
