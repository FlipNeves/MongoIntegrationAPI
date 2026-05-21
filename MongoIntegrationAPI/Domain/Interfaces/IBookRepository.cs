using MongoIntegrationAPI.Domain.Entities;

namespace MongoIntegrationAPI.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<Book?> GetByIdAsync(string id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<bool> AddAuthorToBookAsync(string bookId, Author author);
        Task<bool> AddCategoryToBookAsync(string bookId, Category category);
    }
}
