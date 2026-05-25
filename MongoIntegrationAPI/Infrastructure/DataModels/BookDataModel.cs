using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoIntegrationAPI.Infrastructure.Attributes;

namespace MongoIntegrationAPI.Infrastructure.DataModels
{
    // Infrastructure layer: storage decisions
    [CollectionName("books")]
    public class BookDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string PublisherId { get; set; } = string.Empty;
        // The database stores nested objects to optimize reads
        public List<AuthorEmbeddedModel> Authors { get; set; } = new List<AuthorEmbeddedModel>();
        public List<CategoryEmbeddedModel> Categories { get; set; } = new List<CategoryEmbeddedModel>();
    }
}
