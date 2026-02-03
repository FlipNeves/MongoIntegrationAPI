using MongoDB.Driver;
using MongoIntegrationAPI.Domain;

namespace MongoIntegrationAPI.Infrastructure
{
    public class InfectedRepository : IInfectedRepository
    {
        private readonly IMongoCollection<Infected> _collection;

        public InfectedRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Infected>(typeof(Infected).Name.ToLower());
        }

        public async Task Add(Infected infected)
        {
            await _collection.InsertOneAsync(infected);
        }

        public async Task<IEnumerable<Infected>> GetAll()
        {
            return await _collection.Find(Builders<Infected>.Filter.Empty).ToListAsync();
        }
    }
}
