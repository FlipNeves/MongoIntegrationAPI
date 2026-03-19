using MongoDB.Driver;
using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Daos
{
    public class PublisherDao : IPublisherDao
    {
        private readonly IMongoCollection<PublisherDataModel> _collection;

        public PublisherDao(IMongoDatabase database)
        {
            _collection = database.GetCollection<PublisherDataModel>("publishers");
        }

        // Add raw ops here
    }
}
