namespace MongoIntegrationAPI.Domain.Entities
{
    public class Book
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PublisherId { get; set; } = string.Empty;
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
