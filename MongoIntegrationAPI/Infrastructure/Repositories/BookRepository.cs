using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Infrastructure.Daos;
using MongoIntegrationAPI.Infrastructure.DataModels;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IGenericRepository<BookDataModel> _genericRepository;
        private readonly IBookDao _bookDao;

        public BookRepository(IGenericRepository<BookDataModel> genericRepository, IBookDao bookDao)
        {
            _genericRepository = genericRepository;
            _bookDao = bookDao;
        }

        public async Task AddBookAsync(Book book)
        {
            var dataModel = new BookDataModel
            {
                Title = book.Title,
                PublisherId = book.PublisherId,
                Authors = book.Authors.Select(a => new AuthorEmbeddedModel { Id = a.Id, Name = a.Name }).ToList(),
                Categories = book.Categories.Select(c => new CategoryEmbeddedModel { Name = c.Name, Description = c.Description }).ToList()
            };

            await _genericRepository.AddAsync(dataModel);
            book.Id = dataModel.Id!; 
        }

        public async Task<Book?> GetByIdAsync(string id)
        {
            var dataModel = await _genericRepository.GetByIdAsync(id);
            if (dataModel == null) return null;

            return MapToDomain(dataModel);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var dataModels = await _genericRepository.GetAllAsync();
            return dataModels.Select(MapToDomain);
        }

        public async Task AddAuthorToBookAsync(string bookId, Author author)
        {
            var embeddedAuthor = new AuthorEmbeddedModel
            {
                Id = author.Id,
                Name = author.Name
            };

            await _bookDao.AddAuthorToBookAtomicAsync(bookId, embeddedAuthor);
        }

        public async Task AddCategoryToBookAsync(string bookId, Category category)
        {
            var embeddedCategory = new CategoryEmbeddedModel
            {
                Name = category.Name,
                Description = category.Description
            };

            await _bookDao.AddCategoryToBookAtomicAsync(bookId, embeddedCategory);
        }

        private Book MapToDomain(BookDataModel dataModel)
        {
            return new Book
            {
                Id = dataModel.Id ?? string.Empty,
                Title = dataModel.Title,
                PublisherId = dataModel.PublisherId,
                Authors = dataModel.Authors.Select(a => new AuthorInBook { Id = a.Id, Name = a.Name }).ToList(),
                Categories = dataModel.Categories.Select(c => new Category { Name = c.Name, Description = c.Description }).ToList()
            };
        }
    }
}
