using MongoIntegrationAPI.Domain.Entities;

namespace MongoIntegrationAPI.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category? GetCategoryByType(int categoryId);
    }
}
