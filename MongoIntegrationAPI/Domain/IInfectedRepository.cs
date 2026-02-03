namespace MongoIntegrationAPI.Domain
{
    public interface IInfectedRepository
    {
        Task Add(Infected infected);
        Task<IEnumerable<Infected>> GetAll();
    }
}
