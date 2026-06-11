using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStore.Infrastructure.Data;      
using BookStore.Domain.Models;              
using BookStore.Application.DTOs;           

namespace BookStore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BooksController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/books
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _context.Books.Include(b => b.Author).ToListAsync();
        var booksDto = _mapper.Map<List<BookDto>>(books);
        return Ok(booksDto);
    }

    // POST: api/books
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
    {
        var book = _mapper.Map<Book>(dto);

        // Проверяем, что автор существует
        var authorExists = await _context.Authors.AnyAsync(a => a.Id == dto.AuthorId);
        if (!authorExists)
            return BadRequest("Автор не найден");

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var bookDto = _mapper.Map<BookDto>(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, bookDto);
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return NotFound();

        var bookDto = _mapper.Map<BookDto>(book);
        return Ok(bookDto);
    }
}