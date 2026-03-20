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
                var connectionString = configuration["MONGO_URI"] ?? configuration.GetConnectionString("DefaultConnection") ?? configuration["ConnectionString"] ?? "mongodb://localhost:27017";
                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                return new MongoClient(settings);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = configuration["MONGO_DB_NAME"] ?? configuration["DbName"] ?? "mongo-integration-api";
                return client.GetDatabase(databaseName);
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
