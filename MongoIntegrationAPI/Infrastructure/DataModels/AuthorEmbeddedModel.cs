using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoIntegrationAPI.Infrastructure.DataModels
{
    public class AuthorEmbeddedModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
