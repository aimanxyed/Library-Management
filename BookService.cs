using BookLibrary.Data;
using BookLibrary.Entities;

namespace BookLibrary.Services;

/// <summary>
/// TASK 1.5 — Concrete implementation of <see cref="IBookService"/>
/// backed by <see cref="InMemoryStore"/>.
/// </summary>
public class BookService : IBookService
{
    // ── GetAll ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public IEnumerable<Book> GetAll()
    {
        // Return a snapshot so callers cannot mutate the store list directly.
        return InMemoryStore.Books.ToList();
    }

    // ── GetById ───────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Book? GetById(int id)
    {
        return InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
    }

    // ── Create ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Book Create(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        ValidateAuthorExists(book.AuthorId);

        // Auto-generate next Id
        book.Id = InMemoryStore.Books.Count > 0
            ? InMemoryStore.Books.Max(b => b.Id) + 1
            : 1;

        // Wire up navigation property
        book.Author = InMemoryStore.Authors.First(a => a.Id == book.AuthorId);

        InMemoryStore.Books.Add(book);
        book.Author.Books.Add(book);

        return book;
    }

    // ── Update ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public Book Update(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        var existing = InMemoryStore.Books.FirstOrDefault(b => b.Id == book.Id)
            ?? throw new KeyNotFoundException($"Book with Id={book.Id} was not found.");

        ValidateAuthorExists(book.AuthorId);

        // If author changed, update both old and new author navigation collections
        if (existing.AuthorId != book.AuthorId)
        {
            existing.Author?.Books.Remove(existing);

            existing.AuthorId = book.AuthorId;
            existing.Author   = InMemoryStore.Authors.First(a => a.Id == book.AuthorId);
            existing.Author.Books.Add(existing);
        }

        existing.Title     = book.Title;
        existing.Year      = book.Year;
        existing.PageCount = book.PageCount;

        return existing;
    }

    // ── Delete ────────────────────────────────────────────────────────────

    /// <inheritdoc/>
    public void Delete(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id)
            ?? throw new KeyNotFoundException($"Book with Id={id} was not found.");

        // Remove from related collections to keep graph consistent
        book.Author?.Books.Remove(book);

        var bookTagsToRemove = InMemoryStore.BookTags
            .Where(bt => bt.BookId == id)
            .ToList();

        foreach (var bt in bookTagsToRemove)
        {
            bt.Tag?.BookTags.Remove(bt);
            InMemoryStore.BookTags.Remove(bt);
        }

        InMemoryStore.Books.Remove(book);
    }

    // ── Private helpers ───────────────────────────────────────────────────

    private static void ValidateAuthorExists(int authorId)
    {
        if (!InMemoryStore.Authors.Any(a => a.Id == authorId))
            throw new ArgumentException(
                $"Author with Id={authorId} does not exist.", nameof(authorId));
    }
}
