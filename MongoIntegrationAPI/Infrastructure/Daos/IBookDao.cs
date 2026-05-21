using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Daos
{
    public interface IBookDao
    {
        Task<bool> AddAuthorToBookAtomicAsync(string bookId, AuthorEmbeddedModel author);
        Task<bool> AddCategoryToBookAtomicAsync(string bookId, CategoryEmbeddedModel category);
    }
}
