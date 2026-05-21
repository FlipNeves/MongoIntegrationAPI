using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly IGenericRepository<PublisherDataModel> _genericRepository;

        public PublisherRepository(IGenericRepository<PublisherDataModel> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddAsync(Publisher publisher)
        {
            var dataModel = new PublisherDataModel
            {
                Name = publisher.Name
            };

            await _genericRepository.AddAsync(dataModel);
            publisher.Id = dataModel.Id!;
        }

        public async Task<Publisher?> GetByIdAsync(string id)
        {
            var dataModel = await _genericRepository.GetByIdAsync(id);
            return dataModel == null ? null : MapToDomain(dataModel);
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var dataModels = await _genericRepository.GetAllAsync();
            return dataModels.Select(MapToDomain);
        }

        private static Publisher MapToDomain(PublisherDataModel dataModel)
        {
            return new Publisher
            {
                Id = dataModel.Id ?? string.Empty,
                Name = dataModel.Name
            };
        }
    }
}
