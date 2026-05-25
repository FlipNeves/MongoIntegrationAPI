# MongoIntegrationAPI

Born from experiences where past MongoDB integrations inadvertently locked down architectural flexibility (oh, those experiences...).

It aims to demonstrate best practices for MongoDB integration in .NET, keeping the domain free of driver concerns and isolating persistence decisions in the infrastructure layer. It serves as a practical example for developers looking to build scalable and maintainable applications with .NET and MongoDB without falling into the trap of "infrastructure infection".

## Core Concepts & Architectural Patterns

This project is a practical guide that showcases several key architectural concepts:

- **Clean Architecture.** The project is organized into distinct layers, promoting separation of concerns:
    - `Domain`: Pure POCO entities, enums and repository interfaces. No MongoDB driver references.
    - `Infrastructure`: DataModels (BSON-decorated), Generic Repository, Specific Repositories (translators), DAOs and MongoDB configuration.
    - `Controllers` + `Model`: API entry point and DTOs.
- **Translation Layer.** Specific repositories (`BookRepository`, `AuthorRepository`, ...) translate between domain entities and `*DataModel` classes. The domain never sees `ObjectId`, `[BsonId]` or any other driver attribute.
- **Generic Repository.** `GenericRepository<TDocument>` provides reusable CRUD over any DataModel. The target collection is resolved from a custom `[CollectionName("...")]` attribute via reflection, so each DataModel declares its own collection name.
- **DAOs for atomic operations.** When loading the whole document into memory just to mutate a single embedded field would be wasteful, a dedicated DAO (e.g. `BookDao`) issues surgical updates like `$push` directly against the MongoDB driver.
- **Embedded documents as a domain decision.** `Book` carries `Authors` and `Categories` because the business operation loads them together — the embedding shape lives in the DataModel, not in the domain.
- **Dependency Injection.** All services (Mongo client, database, generic repository, specific repositories and DAOs) are wired up in `MongoDbSetup.AddMongoDb`.

## Project Structure

```
MongoIntegrationAPI/
├── Controllers/
│   ├── BooksController.cs
│   ├── AuthorsController.cs
│   ├── PublishersController.cs
│   └── CategoriesController.cs
├── Domain/
│   ├── Entities/                  // Pure POCOs: Book, Author, Publisher, Category
│   ├── Enums/                     // CategoryType
│   └── Interfaces/                // IBookRepository, IAuthorRepository, IPublisherRepository, ICategoryRepository
├── Infrastructure/
│   ├── Attributes/
│   │   └── CollectionNameAttribute.cs
│   ├── DataModels/                // BookDataModel, AuthorDataModel, PublisherDataModel, AuthorEmbeddedModel, CategoryEmbeddedModel
│   ├── Repositories/
│   │   ├── IGenericRepository.cs
│   │   ├── GenericRepository.cs
│   │   ├── BookRepository.cs      // Translates Domain <-> DataModel and delegates to the DAO when needed
│   │   ├── AuthorRepository.cs
│   │   ├── PublisherRepository.cs
│   │   └── CategoryRepository.cs  // In-memory catalogue driven by the CategoryType enum
│   ├── Daos/
│   │   ├── IBookDao.cs
│   │   ├── BookDao.cs             // Atomic $push for authors/categories
│   │   ├── IPublisherDao.cs
│   │   └── PublisherDao.cs
│   └── MongoDbSetup.cs            // DI registration and Mongo conventions
├── Model/                         // DTOs: BookDtos, PublisherDtos, ...
├── Program.cs                     // Loads .env, registers services, maps controllers
├── appsettings.json
└── appsettings.Development.json
```

## API Endpoints

| Method | Route                       | Description                                                |
| ------ | --------------------------- | ---------------------------------------------------------- |
| POST   | `/books`                    | Create a book (embedding authors and categories).          |
| GET    | `/books`                    | List all books.                                            |
| GET    | `/books/{id}`               | Get a book by id.                                          |
| POST   | `/books/{id}/authors`       | Atomically push an author into the book (via `BookDao`).   |
| POST   | `/books/{id}/categories`    | Atomically push a category into the book (via `BookDao`).  |
| POST   | `/authors`                  | Create an author.                                          |
| GET    | `/authors`                  | List all authors.                                          |
| GET    | `/authors/{id}`             | Get an author by id.                                       |
| POST   | `/publishers`               | Create a publisher.                                        |
| GET    | `/publishers`               | List all publishers.                                       |
| GET    | `/publishers/{id}`          | Get a publisher by id.                                     |
| GET    | `/categories`               | List the static category catalogue (enum-driven).          |
| GET    | `/categories/{id}`          | Get a category by its `CategoryType` id.                   |

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A running MongoDB instance (local, Docker, or a cloud service like MongoDB Atlas).

### 1. Configuration

The application resolves the connection in the following order (see `Infrastructure/MongoDbSetup.cs`):

1. `MONGO_URI` (environment variable or `.env` file).
2. `ConnectionStrings:DefaultConnection`.
3. `ConnectionString` (top-level key in `appsettings.json`).
4. Falls back to `mongodb://localhost:27017`.

The database name is resolved similarly via `MONGO_DB_NAME` → `DbName` → `mongo-integration-api`.

The project uses [`DotNetEnv`](https://www.nuget.org/packages/DotNetEnv), so a `.env` file placed next to the executable is picked up at startup. Example:

```env
MONGO_URI=mongodb://localhost:27017
MONGO_DB_NAME=library
```

Or configure it directly in `appsettings.json`:

```json
{
  "ConnectionString": "mongodb://localhost:27017",
  "DbName": "library"
}
```

### 2. Running the Application

From the solution root:

```bash
dotnet run --project MongoIntegrationAPI
```

In Development the Swagger UI is exposed at `/swagger`.
