using Library_Management.Data;
using Library_Management.DTO;
using Library_Management.Exceptions;
using Library_Management.Mappers;

namespace Library_Management.Services;

/// <summary>
/// In-memory implementation of IBookService.
/// Handles filtering, pagination, mapping, and validation.
/// Entities never leave this class — only DTOs cross the boundary.
/// </summary>
public class BookService : IBookService
{
    // ── GetAllAsync ──────────────────────────────────────────────────────

    public async Task<IEnumerable<BookResponseDTO>> GetAllAsync(
        string? author = null,
        int page = 1,
        int pageSize = 10)
    {
        // Guard: page and pageSize must be positive
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = InMemoryStore.Books.AsEnumerable();

        // Optional filter — case-insensitive author name contains
        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b =>
                b.Author != null &&
                b.Author.Name.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        // Pagination — skip previous pages, take current page
        var paged = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return await Task.FromResult(BookMapper.ToResponseList(paged));
    }

    // ── GetByIdAsync ─────────────────────────────────────────────────────

    public async Task<BookResponseDTO> GetByIdAsync(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id)
            ?? throw new BookNotFoundException(id);

        return await Task.FromResult(BookMapper.ToResponse(book));
    }

    // ── CreateAsync ──────────────────────────────────────────────────────

    public async Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        ValidateAuthorExists(dto.AuthorId);

        // Map DTO → entity
        var book = BookMapper.ToEntity(dto);

        // Assign Id
        book.Id = InMemoryStore.Books.Count > 0
            ? InMemoryStore.Books.Max(b => b.Id) + 1
            : 1;

        // Wire navigation property
        book.Author = InMemoryStore.Authors.First(a => a.Id == dto.AuthorId);

        InMemoryStore.Books.Add(book);
        book.Author.Books.Add(book);

        return await Task.FromResult(BookMapper.ToResponse(book));
    }

    // ── UpdateAsync ──────────────────────────────────────────────────────

    public async Task<BookResponseDTO> UpdateAsync(BookUpdateDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var existing = InMemoryStore.Books.FirstOrDefault(b => b.Id == dto.Id)
            ?? throw new BookNotFoundException(dto.Id);

        ValidateAuthorExists(dto.AuthorId);

        // If author changed, update navigation collections on both sides
        if (existing.AuthorId != dto.AuthorId)
        {
            existing.Author?.Books.Remove(existing);
            existing.Author = InMemoryStore.Authors.First(a => a.Id == dto.AuthorId);
            existing.Author.Books.Add(existing);
        }

        // Apply DTO fields onto existing entity
        BookMapper.ApplyUpdate(dto, existing);

        return await Task.FromResult(BookMapper.ToResponse(existing));
    }

    // ── DeleteAsync ──────────────────────────────────────────────────────

    public Task DeleteAsync(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id)
            ?? throw new BookNotFoundException(id);

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

        return Task.CompletedTask;
    }

    // ── Private helpers ──────────────────────────────────────────────────

    private static void ValidateAuthorExists(int authorId)
    {
        if (!InMemoryStore.Authors.Any(a => a.Id == authorId))
            throw new ArgumentException(
                $"Author with Id={authorId} does not exist.", nameof(authorId));
    }
}