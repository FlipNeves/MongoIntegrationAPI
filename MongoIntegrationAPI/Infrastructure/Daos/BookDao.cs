using MongoDB.Driver;
using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Daos
{
    public class BookDao : IBookDao
    {
        private readonly IMongoCollection<BookDataModel> _collection;

        public BookDao(IMongoDatabase database)
        {
            _collection = database.GetCollection<BookDataModel>("books");
        }

        public async Task<bool> AddAuthorToBookAtomicAsync(string bookId, AuthorEmbeddedModel author)
        {
            var filter = Builders<BookDataModel>.Filter.Eq(b => b.Id, bookId);
            var update = Builders<BookDataModel>.Update.Push(b => b.Authors, author);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.MatchedCount > 0;
        }

        public async Task<bool> AddCategoryToBookAtomicAsync(string bookId, CategoryEmbeddedModel category)
        {
            var filter = Builders<BookDataModel>.Filter.Eq(b => b.Id, bookId);
            var update = Builders<BookDataModel>.Update.Push(b => b.Categories, category);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.MatchedCount > 0;
        }
    }
}
