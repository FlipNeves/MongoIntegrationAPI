using Microsoft.AspNetCore.Mvc;
using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDto request)
        {
            var categories = request.Categories.Select(x => _categoryRepository.GetCategoryByType((int)x)).ToList();
            var book = new Book
            {
                Title = request.Title,
                PublisherId = request.PublisherId,
                Authors = request.Authors.SelectMany(x => new List<AuthorInBook>
                {
                    new AuthorInBook
                    {
                        Name = x.Name,
                        Id = x.Id
                    }
                }).ToList(),
                Categories = categories!
            };
            

            await _bookRepository.AddBookAsync(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(string id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepository.GetAllAsync();
            return Ok(books);
        }

        [HttpPost("{id}/authors")]
        public async Task<IActionResult> AddAuthorToBook(string id, [FromBody] AuthorDto request)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return NotFound("Book not found");

            var author = new Author
            {
                Name = request.Name,
            };

            await _bookRepository.AddAuthorToBookAsync(id, author);

            return Ok(new { message = "Author created and linked successfully", author });
        }

        [HttpPost("{id}/categories")]
        public async Task<IActionResult> AddCategoryToBook(string id, [FromBody] CategoryDto request)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return NotFound("Book not found");

            var category = _categoryRepository.GetCategoryByType(request.CategoryId);
            if (category == null) return BadRequest("Invalid Category ID");

            await _bookRepository.AddCategoryToBookAsync(id, category);

            return Ok(new { message = "Category added successfully", category });
        }
    }
}
