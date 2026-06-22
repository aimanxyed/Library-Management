using Library_Management.DTO;
using Library_Management.Entities;
using Library_Management.Exceptions;
using Library_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management.Controllers;

/// <summary>
/// TASK 2.3 — Exposes book endpoints over HTTP.
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // ── GET /api/books?author=Herbert&page=1&pageSize=5 ───────────────────

    /// <summary>
    /// Returns a paged list of books, optionally filtered by author name.
    /// </summary>
    /// <param name="author">Optional partial author name filter (case-insensitive).</param>
    /// <param name="page">Page number, 1-based. Defaults to 1.</param>
    /// <param name="pageSize">Items per page. Defaults to 10.</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAll(
        [FromQuery] string? author = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.GetAllAsync(author, page, pageSize);
        return Ok(books);
    }

    // ── GET /api/books/{id} ───────────────────────────────────────────────

    /// <summary>
    /// Returns a single book by Id.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponseDTO>> GetById(int id)
    {
        try
        {
            var book = await _bookService.GetByIdAsync(id);
            return Ok(book);
        }
        catch (BookNotFoundException ex)
        {
            return NotFound(new { ex.Message, ex.BookId });
        }
    }

    // ── POST /api/books ───────────────────────────────────────────────────

    /// <summary>
    /// Creates a new book. Returns 201 Created with Location header.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BookResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookCreateDTO dto)
    {
        try
        {
            var created = await _bookService.CreateAsync(dto);
            // 201 + Location: /api/books/{id}
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    // ── PUT /api/books/{id} ───────────────────────────────────────────────

    /// <summary>
    /// Fully replaces an existing book. Id in route must match Id in body.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BookResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookResponseDTO>> Update(int id, [FromBody] BookUpdateDTO dto)
    {
        // Route Id and body Id must agree — prevents silent Id mismatch bugs
        if (id != dto.Id)
            return BadRequest(new { Message = $"Route Id={id} does not match body Id={dto.Id}." });

        try
        {
            var updated = await _bookService.UpdateAsync(dto);
            return Ok(updated);
        }
        catch (BookNotFoundException ex)
        {
            return NotFound(new { ex.Message, ex.BookId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    // ── DELETE /api/books/{id} ────────────────────────────────────────────

    /// <summary>
    /// Deletes a book by Id. Returns 204 No Content on success.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bookService.DeleteAsync(id);
            return NoContent(); // 204 — success, nothing to return
        }
        catch (BookNotFoundException ex)
        {
            return NotFound(new { ex.Message, ex.BookId });
        }
    }
}