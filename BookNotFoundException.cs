namespace Library_Management.Exceptions;

/// <summary>
/// TASK 2.5 — Thrown when a requested Book cannot be found by Id.
/// Kept distinct from generic KeyNotFoundException so callers (e.g. middleware,
/// controllers) can map it to a specific HTTP status code (404) unambiguously.
/// </summary>
public class BookNotFoundException : Exception
{
    public int BookId { get; }

    public BookNotFoundException(int bookId)
        : base($"Book with Id={bookId} was not found.")
    {
        BookId = bookId;
    }

    public BookNotFoundException(int bookId, string message)
        : base(message)
    {
        BookId = bookId;
    }

    public BookNotFoundException(int bookId, string message, Exception innerException)
        : base(message, innerException)
    {
        BookId = bookId;
    }
}
