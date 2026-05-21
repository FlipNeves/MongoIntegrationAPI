using System.ComponentModel;
using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Enums;
using MongoIntegrationAPI.Domain.Interfaces;

namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    // Backed by an enum, not a database — methods are intentionally synchronous.
    public class CategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            var enumValues = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>();

            foreach (var value in enumValues)
            {
                categories.Add(new Category
                {
                    Name = value.ToString(),
                    Description = GetEnumDescription(value)
                });
            }

            return categories;
        }

        public Category? GetCategoryByType(int categoryId)
        {
            if (!Enum.IsDefined(typeof(CategoryType), categoryId))
                return null;

            var categoryType = (CategoryType)categoryId;
            
            return new Category
            {
                Name = categoryType.ToString(),
                Description = GetEnumDescription(categoryType)
            };
        }

        private string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return string.Empty;
            
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
