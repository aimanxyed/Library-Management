using BookLibrary.Data;
using BookLibrary.Entities;
using BookLibrary.Queries;
using BookLibrary.Services;

// ═══════════════════════════════════════════════════════
//   BookLibrary — Tasks 1.1 → 1.5 Demo Entry Point
// ═══════════════════════════════════════════════════════

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ── Show seeded store (Task 1.2) ─────────────────────────────────────────
Console.WriteLine("══════════════════════════════════════════════");
Console.WriteLine("            SEEDED DATA SUMMARY               ");
Console.WriteLine("══════════════════════════════════════════════");
Console.WriteLine($"\n Authors  ({InMemoryStore.Authors.Count}):");
InMemoryStore.Authors.ForEach(a => Console.WriteLine($"  {a}"));

Console.WriteLine($"\n Books    ({InMemoryStore.Books.Count}):");
InMemoryStore.Books.ForEach(b => Console.WriteLine($"  {b}"));

Console.WriteLine($"\n Tags     ({InMemoryStore.Tags.Count}):");
InMemoryStore.Tags.ForEach(t => Console.WriteLine($"  {t}"));

Console.WriteLine($"\n BookTags ({InMemoryStore.BookTags.Count}):");
InMemoryStore.BookTags.ForEach(bt =>
    Console.WriteLine($"  {bt.Book?.Title,-35} ← {bt.Tag?.Name}"));

Console.WriteLine();

// ── LINQ Queries (Task 1.3) ──────────────────────────────────────────────
BookQueries.RunAll();

// ── BookService CRUD demo (Tasks 1.4 / 1.5) ─────────────────────────────
Console.WriteLine("══════════════════════════════════════════════");
Console.WriteLine("           BOOKSERVICE CRUD DEMO               ");
Console.WriteLine("══════════════════════════════════════════════\n");

IBookService service = new BookService();

// GetAll
Console.WriteLine($"GetAll() → {service.GetAll().Count()} books\n");

// GetById
var found = service.GetById(4);
Console.WriteLine($"GetById(4) → {found}\n");

// GetById — not found
var missing = service.GetById(99);
Console.WriteLine($"GetById(99) → {(missing is null ? "null (not found)" : missing)}\n");

// Create
var newBook = new Book
{
    Title     = "God Emperor of Dune",
    Year      = 1981,
    PageCount = 496,
    AuthorId  = 2   // Frank Herbert
};
var created = service.Create(newBook);
Console.WriteLine($"Create() → {created}");
Console.WriteLine($"Store now has {InMemoryStore.Books.Count} books\n");

// Update
created.Title     = "God Emperor of Dune (Updated)";
created.PageCount = 500;
var updated = service.Update(created);
Console.WriteLine($"Update() → {updated}\n");

// Delete
service.Delete(created.Id);
Console.WriteLine($"Delete(Id={created.Id}) done.");
Console.WriteLine($"Store now has {InMemoryStore.Books.Count} books\n");

// Exception demos
Console.WriteLine("── Exception safety demos ──");
try
{
    service.GetById(99); // returns null, no throw
    service.Delete(99);  // throws
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"  KeyNotFoundException: {ex.Message}");
}

try
{
    service.Create(new Book { Title = "Ghost", Year = 2024, PageCount = 300, AuthorId = 99 });
}
catch (ArgumentException ex)
{
    Console.WriteLine($"  ArgumentException: {ex.Message}");
}

Console.WriteLine("\nAll tasks complete.");
