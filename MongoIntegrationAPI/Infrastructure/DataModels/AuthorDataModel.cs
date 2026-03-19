using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoIntegrationAPI.Infrastructure.Attributes;

namespace MongoIntegrationAPI.Infrastructure.DataModels
{
    [CollectionName("authors")]
    public class AuthorDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Bibliography { get; set; } = string.Empty;
    }
}
