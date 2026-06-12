using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;
using BookStore.Application.Services;

namespace BookStore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    // GET: api/books/search?title=Война&author=Толстой
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? title, [FromQuery] string? author)
    {
        var books = await _bookService.SearchAsync(title, author);
        return Ok(books);
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
    {
        var book = await _bookService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT: api/books/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateBookDto dto)
    {
        var book = await _bookService.UpdateAsync(id, dto);
        if (book == null) return NotFound();
        return Ok(book);
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
    // GET: api/books/paginated?page=1&pageSize=10
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _bookService.GetPaginatedAsync(page, pageSize);
        return Ok(result);
    }
}