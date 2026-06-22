namespace Library_Management.Entities;


public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public int PageCount { get; set; }

    // Foreign key
    public int AuthorId { get; set; }

    // Navigation property → parent Author
    public Author? Author { get; set; }

    // Navigation property → many-to-many Tags via BookTag
    public ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();

    //public override string ToString() =>
    //    $"[Book #{Id}] \"{Title}\" ({Year}) — {PageCount} pages — AuthorId={AuthorId}";
}
