using Library_Management.Data;
using Library_Management.Entities;

namespace Library_Management.Queries;

/// <summary>
/// TASK 1.3 — Eight LINQ query demonstrations against InMemoryStore.
/// Call RunAll() to execute and print every query.
/// </summary>
public static class BookQueries
{
    public static void RunAll()
    {
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("           LINQ QUERY DEMONSTRATIONS           ");
        Console.WriteLine("══════════════════════════════════════════════\n");

        Query1_FilterByAuthor();
        Query2_TitlesSortedAlpha();
        Query3_GroupByAuthor();
        Query4_AveragePageCount();
        Query5_AnyOver500Pages();
        Query6_FirstOrDefaultById();
        Query7_JoinBooksAndAuthors();
        Query8_Top3LongestBooks();
    }

    // ── Q1: Filter books by a specific author name ───────────────────────
    private static void Query1_FilterByAuthor()
    {
        Console.WriteLine("── Q1: Books by Frank Herbert ──");

        var results = from b in InMemoryStore.Books
                      join a in InMemoryStore.Authors on b.AuthorId equals a.Id
                      where a.Name == "Frank Herbert"
                      select b;

        foreach (var b in results)
            Console.WriteLine($"  {b}");

        Console.WriteLine();
    }

    // ── Q2: Select all titles, sorted alphabetically ─────────────────────
    private static void Query2_TitlesSortedAlpha()
    {
        Console.WriteLine("── Q2: All titles sorted A→Z ──");

        var titles = InMemoryStore.Books
            .Select(b => b.Title)
            .OrderBy(t => t);

        foreach (var t in titles)
            Console.WriteLine($"  • {t}");

        Console.WriteLine();
    }

    // ── Q3: Group books by author, show count per author ─────────────────
    private static void Query3_GroupByAuthor()
    {
        Console.WriteLine("── Q3: Books grouped by Author ──");

        var groups = InMemoryStore.Books
            .GroupBy(b => b.AuthorId)
            .Select(g => new
            {
                Author = InMemoryStore.Authors.First(a => a.Id == g.Key).Name,
                Count  = g.Count(),
                Titles = g.Select(b => b.Title).ToList()
            });

        foreach (var g in groups)
        {
            Console.WriteLine($"  {g.Author} ({g.Count} books):");
            g.Titles.ForEach(t => Console.WriteLine($"    - {t}"));
        }

        Console.WriteLine();
    }

    // ── Q4: Average page count across all books ───────────────────────────
    private static void Query4_AveragePageCount()
    {
        Console.WriteLine("── Q4: Average page count ──");

        double avg = InMemoryStore.Books.Average(b => b.PageCount);
        Console.WriteLine($"  Average pages: {avg:F1}");
        Console.WriteLine();
    }

    // ── Q5: Any book with more than 500 pages? ────────────────────────────
    private static void Query5_AnyOver500Pages()
    {
        Console.WriteLine("── Q5: Any book > 500 pages? ──");

        bool hasLongBook = InMemoryStore.Books.Any(b => b.PageCount > 500);
        Console.WriteLine($"  Result: {hasLongBook}");

        if (hasLongBook)
        {
            var longBooks = InMemoryStore.Books
                .Where(b => b.PageCount > 500)
                .Select(b => $"{b.Title} ({b.PageCount} pages)");
            Console.WriteLine($"  Books: {string.Join(", ", longBooks)}");
        }

        Console.WriteLine();
    }

    // ── Q6: FirstOrDefault — find book by Id ─────────────────────────────
    private static void Query6_FirstOrDefaultById()
    {
        Console.WriteLine("── Q6: FirstOrDefault by Id (Id=4) ──");

        int searchId = 4;
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == searchId);

        Console.WriteLine(book is not null
            ? $"  Found: {book}"
            : $"  No book found with Id={searchId}");

        Console.WriteLine();
    }

    // ── Q7: Join books with authors (manual join on foreign key) ──────────
    private static void Query7_JoinBooksAndAuthors()
    {
        Console.WriteLine("── Q7: Join Books + Authors ──");

        var joined = from b in InMemoryStore.Books
                     join a in InMemoryStore.Authors on b.AuthorId equals a.Id
                     orderby a.Name, b.Year
                     select new { BookTitle = b.Title, AuthorName = a.Name, b.Year, b.PageCount };

        foreach (var row in joined)
            Console.WriteLine($"  [{row.AuthorName}]  {row.BookTitle}  ({row.Year}, {row.PageCount}pp)");

        Console.WriteLine();
    }

    // ── Q8: Top 3 longest books by page count ────────────────────────────
    private static void Query8_Top3LongestBooks()
    {
        Console.WriteLine("── Q8: Top 3 longest books ──");

        var top3 = InMemoryStore.Books
            .OrderByDescending(b => b.PageCount)
            .Take(3);

        int rank = 1;
        foreach (var b in top3)
            Console.WriteLine($"  #{rank++}  {b.Title}  ({b.PageCount} pages)");

        Console.WriteLine();
    }
}
