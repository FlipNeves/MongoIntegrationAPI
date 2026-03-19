using MongoIntegrationAPI.Domain.Entities;

namespace MongoIntegrationAPI.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);
        Task<Book?> GetByIdAsync(string id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task AddAuthorToBookAsync(string bookId, Author author);
        Task AddCategoryToBookAsync(string bookId, Category category);
    }
}
