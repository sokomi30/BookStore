using Microsoft.AspNetCore.Mvc;
using BookStore.Application.DTOs;
using BookStore.Application.Services;

namespace BookStore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var authors = await _authorService.GetAllAsync();
            return Ok(authors);
        }

        // GET: api/authors/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        // POST: api/authors
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto dto)
        {
            var author = await _authorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }

        // PUT: api/authors/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
        {
            var author = await _authorService.UpdateAsync(id, dto);
            if (author == null) return NotFound();
            return Ok(author);
        }

        // DELETE: api/authors/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _authorService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}