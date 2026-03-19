using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Infrastructure.Daos;
using MongoIntegrationAPI.Infrastructure.Repositories;

namespace MongoIntegrationAPI.Infrastructure
{
    public static class MongoDbSetup
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"] ?? "mongodb://localhost:27017"));
                return new MongoClient(settings);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(configuration["DbName"] ?? "mongo-integration-api");
            });

            // Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // DAOs
            services.AddScoped<IBookDao, BookDao>();
            services.AddScoped<IPublisherDao, PublisherDao>();

            // Repositories
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
        }
    }
}
