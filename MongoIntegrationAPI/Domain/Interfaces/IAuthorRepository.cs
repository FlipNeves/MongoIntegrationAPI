using MongoIntegrationAPI.Domain.Entities;

namespace MongoIntegrationAPI.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task AddAsync(Author author);
        Task<Author?> GetByIdAsync(string id);
        Task<IEnumerable<Author>> GetAllAsync();
    }
}
