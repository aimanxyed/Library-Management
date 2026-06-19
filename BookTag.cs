namespace BookLibrary.Entities;

/// <summary>
/// Join / bridge entity for the many-to-many relationship between Book and Tag.
/// </summary>
public class BookTag
{
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int TagId { get; set; }
    public Tag? Tag { get; set; }

    public override string ToString() => $"BookTag(BookId={BookId}, TagId={TagId})";
}
