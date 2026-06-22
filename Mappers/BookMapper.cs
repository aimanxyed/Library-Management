using Library_Management.Entities;
using Library_Management.DTO;

namespace Library_Management.Mappers
{ //The key rule now: entities go in, DTOs come out — the Book entity never crosses the service boundary.
    public static class BookMapper
    {
        // ── ToEntity ─────────────────────────────────────────────────────────
        // Converts an incoming CREATE request into a fresh Book entity.
        // Id is intentionally left at 0 — BookService assigns the real Id.
        // Author navigation property is null here — BookService wires it up
        // after confirming the AuthorId exists in the store.

        public static Book ToEntity(BookCreateDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new Book
            {
                Id = 0,           // assigned by BookService
                Title = dto.Title.Trim(),
                Year = dto.Year,
                PageCount = dto.PageCount,
                AuthorId = dto.AuthorId,
                Author = null         // wired up by BookService
            };
        }

        // ── ApplyUpdate ───────────────────────────────────────────────────────
        // Applies fields from an UPDATE request onto an existing Book entity.
        // Mutates the entity in place (ref semantics) — no new object created.
        // Returns the same entity so call sites can chain if needed.
        // Id on the entity is never overwritten from the DTO — the route Id
        // is the authority; the DTO Id is just for validation consistency.

        public static Book ApplyUpdate(BookUpdateDTO dto, Book existing)
        {
            ArgumentNullException.ThrowIfNull(dto);
            ArgumentNullException.ThrowIfNull(existing);

            existing.Title = dto.Title.Trim();
            existing.Year = dto.Year;
            existing.PageCount = dto.PageCount;
            existing.AuthorId = dto.AuthorId;

            return existing;
        }

        // ── ToResponse ────────────────────────────────────────────────────────
        // Projects a Book entity into a clean response DTO.
        // Flattens Author.Name directly onto the DTO so the caller never
        // sees a nested Author object or a circular reference.

        public static BookResponseDTO ToResponse(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);

            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                PageCount = book.PageCount,
                AuthorId = book.AuthorId,
                AuthorName = book.Author?.Name ?? string.Empty
            };
        }

        // ── ToResponseList ────────────────────────────────────────────────────
        // Convenience overload — maps a collection in one call.
        // Used in GetAll so the controller stays a one-liner.

        public static IEnumerable<BookResponseDTO> ToResponseList(IEnumerable<Book> books)
        {
            ArgumentNullException.ThrowIfNull(books);

            return books.Select(ToResponse);
        }
    }
}
