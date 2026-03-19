using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Infrastructure.DataModels;

namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IGenericRepository<AuthorDataModel> _genericRepository;

        public AuthorRepository(IGenericRepository<AuthorDataModel> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task AddAsync(Author author)
        {
            var dataModel = new AuthorDataModel
            {
                Name = author.Name,
                Bibliography = author.Bibliography
            };

            await _genericRepository.AddAsync(dataModel);
            author.Id = dataModel.Id!;
        }

        public async Task<Author?> GetByIdAsync(string id)
        {
            var dataModel = await _genericRepository.GetByIdAsync(id);
            if (dataModel == null) return null;

            return new Author
            {
                Id = dataModel.Id ?? string.Empty,
                Name = dataModel.Name,
                Bibliography = dataModel.Bibliography
            };
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var dataModels = await _genericRepository.GetAllAsync();
            return dataModels.Select(dm => new Author
            {
                Id = dm.Id ?? string.Empty,
                Name = dm.Name,
                Bibliography = dm.Bibliography
            });
        }
    }
}
