using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Daos
{
    public interface IBookDao
    {
        Task AddAuthorToBookAtomicAsync(string bookId, AuthorEmbeddedModel author);
        Task AddCategoryToBookAtomicAsync(string bookId, CategoryEmbeddedModel category);
    }
}
