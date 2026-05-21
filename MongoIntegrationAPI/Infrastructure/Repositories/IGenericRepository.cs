namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    public interface IGenericRepository<TDocument> where TDocument : class
    {
        Task AddAsync(TDocument document);
        Task<TDocument?> GetByIdAsync(string id);
        Task<IEnumerable<TDocument>> GetAllAsync();
    }
}
