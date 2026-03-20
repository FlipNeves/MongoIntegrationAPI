namespace MongoIntegrationAPI.Domain.Entities
{
    public class Book
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PublisherId { get; set; } = string.Empty;
        public List<AuthorInBook> Authors { get; set; } = new List<AuthorInBook>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }

    public class AuthorInBook
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
