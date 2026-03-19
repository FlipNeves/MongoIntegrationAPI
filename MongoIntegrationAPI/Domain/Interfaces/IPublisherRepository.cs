using MongoIntegrationAPI.Domain.Entities;

namespace MongoIntegrationAPI.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        Task AddPublisherAsync(Publisher publisher);
        Task<Publisher?> GetByIdAsync(string id);
        Task<IEnumerable<Publisher>> GetAllAsync();
    }
}
