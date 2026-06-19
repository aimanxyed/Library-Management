using BookLibrary.Entities;

namespace BookLibrary.Services;

/// <summary>
/// TASK 1.4 — Contract for book CRUD operations.
/// </summary>
public interface IBookService
{
    /// <summary>Returns all books in the store.</summary>
    IEnumerable<Book> GetAll();

    /// <summary>Returns a single book by primary key, or null if not found.</summary>
    Book? GetById(int id);

    /// <summary>
    /// Adds a new book. Assigns a generated Id.
    /// Throws <see cref="ArgumentException"/> if AuthorId does not exist.
    /// Returns the created book with its new Id.
    /// </summary>
    Book Create(Book book);

    /// <summary>
    /// Updates an existing book.
    /// Throws <see cref="KeyNotFoundException"/> when the Id is not found.
    /// Throws <see cref="ArgumentException"/> if the new AuthorId does not exist.
    /// Returns the updated book.
    /// </summary>
    Book Update(Book book);

    /// <summary>
    /// Deletes the book with the given Id.
    /// Throws <see cref="KeyNotFoundException"/> when the Id is not found.
    /// </summary>
    void Delete(int id);
}
