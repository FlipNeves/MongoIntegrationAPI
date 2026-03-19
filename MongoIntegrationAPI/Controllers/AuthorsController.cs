using Microsoft.AspNetCore.Mvc;
using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto request)
        {
            var author = new Author
            {
                Name = request.Name,
                Bibliography = request.Bibliography
            };

            await _authorRepository.AddAsync(author);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(string id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null) return NotFound();

            return Ok(author);
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            return Ok(authors);
        }
    }
}
