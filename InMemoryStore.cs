using BookLibrary.Entities;

namespace BookLibrary.Data;

/// <summary>
/// TASK 1.2 — Static in-memory data store.
/// Seeds 3 Authors, 8 Books, 4 Tags, and 6 BookTag links.
/// Navigation properties are wired up after seeding.
/// </summary>
public static class InMemoryStore
{
    // ─────────────────────────────────────────────
    // Raw collections
    // ─────────────────────────────────────────────

    public static List<Author> Authors { get; } = new();
    public static List<Book>   Books   { get; } = new();
    public static List<Tag>    Tags    { get; } = new();
    public static List<BookTag> BookTags { get; } = new();

    // ─────────────────────────────────────────────
    // Seed
    // ─────────────────────────────────────────────

    static InMemoryStore()
    {
        SeedAuthors();
        SeedBooks();
        SeedTags();
        SeedBookTags();
        WireNavigationProperties();
    }

    private static void SeedAuthors()
    {
        Authors.AddRange(new[]
        {
            new Author { Id = 1, Name = "George Orwell"       },
            new Author { Id = 2, Name = "Frank Herbert"       },
            new Author { Id = 3, Name = "Fyodor Dostoevsky"   },
        });
    }

    private static void SeedBooks()
    {
        Books.AddRange(new[]
        {
            new Book { Id = 1, Title = "Nineteen Eighty-Four",        Year = 1949, PageCount = 328,  AuthorId = 1 },
            new Book { Id = 2, Title = "Animal Farm",                 Year = 1945, PageCount = 112,  AuthorId = 1 },
            new Book { Id = 3, Title = "Homage to Catalonia",         Year = 1938, PageCount = 232,  AuthorId = 1 },
            new Book { Id = 4, Title = "Dune",                        Year = 1965, PageCount = 688,  AuthorId = 2 },
            new Book { Id = 5, Title = "Dune Messiah",                Year = 1969, PageCount = 336,  AuthorId = 2 },
            new Book { Id = 6, Title = "Children of Dune",            Year = 1976, PageCount = 444,  AuthorId = 2 },
            new Book { Id = 7, Title = "Crime and Punishment",        Year = 1866, PageCount = 551,  AuthorId = 3 },
            new Book { Id = 8, Title = "The Brothers Karamazov",      Year = 1880, PageCount = 796,  AuthorId = 3 },
        });
    }

    private static void SeedTags()
    {
        Tags.AddRange(new[]
        {
            new Tag { Id = 1, Name = "Dystopia"    },
            new Tag { Id = 2, Name = "Science Fiction" },
            new Tag { Id = 3, Name = "Classic"     },
            new Tag { Id = 4, Name = "Political"   },
        });
    }

    private static void SeedBookTags()
    {
        // 6 BookTag links
        BookTags.AddRange(new[]
        {
            new BookTag { BookId = 1, TagId = 1 }, // 1984 → Dystopia
            new BookTag { BookId = 1, TagId = 4 }, // 1984 → Political
            new BookTag { BookId = 2, TagId = 4 }, // Animal Farm → Political
            new BookTag { BookId = 4, TagId = 2 }, // Dune → Sci-Fi
            new BookTag { BookId = 7, TagId = 3 }, // Crime & Punishment → Classic
            new BookTag { BookId = 8, TagId = 3 }, // Brothers Karamazov → Classic
        });
    }

    /// <summary>
    /// Wires up navigation properties so object graph is traversable
    /// without a database/ORM.
    /// </summary>
    private static void WireNavigationProperties()
    {
        // Author.Books
        foreach (var book in Books)
        {
            var author = Authors.FirstOrDefault(a => a.Id == book.AuthorId);
            book.Author = author;
            author?.Books.Add(book);
        }

        // BookTag navigation
        foreach (var bt in BookTags)
        {
            bt.Book = Books.FirstOrDefault(b => b.Id == bt.BookId);
            bt.Tag  = Tags.FirstOrDefault(t => t.Id == bt.TagId);

            bt.Book?.BookTags.Add(bt);
            bt.Tag?.BookTags.Add(bt);
        }
    }
}
