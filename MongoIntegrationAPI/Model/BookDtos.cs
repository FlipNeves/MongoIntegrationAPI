using MongoIntegrationAPI.Domain.Enums;

namespace MongoIntegrationAPI.Model
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string PublisherId { get; set; } = string.Empty;
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
        public List<CategoryType> Categories { get; set; } = new List<CategoryType>();
    }

    public class CreateAuthorDto
    {
        public string Name { get; set; } = string.Empty;
        public string Bibliography { get; set; } = string.Empty;
    }

    public class AuthorDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
    }
}
