using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoIntegrationAPI.Infrastructure.Attributes;

namespace MongoIntegrationAPI.Infrastructure.DataModels
{
    [CollectionName("publishers")]
    public class PublisherDataModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
