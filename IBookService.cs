
using Library_Management.DTO;

namespace Library_Management.Services;

/// <summary>
/// TASK 3.2 update — All methods now use DTOs instead of raw entities.
/// The service layer is the boundary: entities stay inside, DTOs cross it.
///
/// GetAllAsync gains filtering (author name) and pagination (page, pageSize)
/// so the controller never does filtering or slicing logic itself.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Returns a paged, optionally filtered list of books.
    /// </summary>
    /// <param name="author">
    /// Optional. Case-insensitive author name filter.
    /// Pass null or empty to return books by all authors.
    /// </param>
    /// <param name="page">1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">Number of items per page. Defaults to 10.</param>
    Task<IEnumerable<BookResponseDTO>> GetAllAsync(
        string? author = null,
        int page = 1,
        int pageSize = 10);

    /// <summary>
    /// Returns a single book by primary key.
    /// </summary>
    /// <exception cref="Exceptions.BookNotFoundException">
    /// Thrown when no book with the given <paramref name="id"/> exists.
    /// </exception>
    Task<BookResponseDTO> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new book from the supplied DTO.
    /// Throws <see cref="ArgumentException"/> if AuthorId does not exist.
    /// Returns the created book as a response DTO.
    /// </summary>
    Task<BookResponseDTO> CreateAsync(BookCreateDTO dto);

    /// <summary>
    /// Replaces an existing book with data from the supplied DTO.
    /// Throws <see cref="Exceptions.BookNotFoundException"/> if Id not found.
    /// Throws <see cref="ArgumentException"/> if AuthorId does not exist.
    /// Returns the updated book as a response DTO.
    /// </summary>
    Task<BookResponseDTO> UpdateAsync(BookUpdateDTO dto);

    /// <summary>
    /// Deletes the book with the given Id.
    /// Throws <see cref="Exceptions.BookNotFoundException"/> if Id not found.
    /// </summary>
    Task DeleteAsync(int id);
}