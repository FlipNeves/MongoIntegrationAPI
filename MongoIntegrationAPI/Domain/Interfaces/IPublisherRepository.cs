using MongoIntegrationAPI.Domain.Entities;

namespace MongoIntegrationAPI.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        Task AddAsync(Publisher publisher);
        Task<Publisher?> GetByIdAsync(string id);
        Task<IEnumerable<Publisher>> GetAllAsync();
    }
}
