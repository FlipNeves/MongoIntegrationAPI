using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Infrastructure.Daos;
using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly IGenericRepository<PublisherDataModel> _genericRepository;
        private readonly IPublisherDao _publisherDao;

        public PublisherRepository(IGenericRepository<PublisherDataModel> genericRepository, IPublisherDao publisherDao)
        {
            _genericRepository = genericRepository;
            _publisherDao = publisherDao;
        }

        public async Task AddPublisherAsync(Publisher publisher)
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
            if (dataModel == null) return null;

            return new Publisher
            {
                Id = dataModel.Id ?? string.Empty,
                Name = dataModel.Name
            };
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var dataModels = await _genericRepository.GetAllAsync();
            return dataModels.Select(dm => new Publisher
            {
                Id = dm.Id ?? string.Empty,
                Name = dm.Name
            });
        }
    }
}
