using Microsoft.AspNetCore.Mvc;
using MongoIntegrationAPI.Domain.Interfaces;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.GetCategoryByType(id);
            if (category == null) return NotFound("Category not found");

            return Ok(category);
        }
    }
}
