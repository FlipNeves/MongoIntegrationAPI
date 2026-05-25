using MongoDB.Bson;
using MongoDB.Driver;
using MongoIntegrationAPI.Infrastructure.Attributes;
using System.Reflection;

namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    public class GenericRepository<TDocument> : IGenericRepository<TDocument> where TDocument : class
    {
        private readonly IMongoCollection<TDocument> _collection;

        public GenericRepository(IMongoDatabase database)
        {
            var collectionName = typeof(TDocument).GetCustomAttribute<CollectionNameAttribute>()?.Name
                ?? typeof(TDocument).Name;

            _collection = database.GetCollection<TDocument>(collectionName);
        }

        public async Task AddAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task<IEnumerable<TDocument>> GetAllAsync()
        {
            return await _collection.Find(Builders<TDocument>.Filter.Empty).ToListAsync();
        }

        public async Task<TDocument?> GetByIdAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", new ObjectId(id));
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(string id, TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", new ObjectId(id));
            await _collection.ReplaceOneAsync(filter, document);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq("_id", new ObjectId(id));
            await _collection.DeleteOneAsync(filter);
        }
    }
}
