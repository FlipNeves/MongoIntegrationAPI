# MongoIntegrationAPI

This project presents a well-structured .NET 8 API template, born from experiences where past MongoDB integrations inadvertently locked down architectural flexibility(oh that experiences...). It aims to demonstrate best practices for MongoDB integration, ensuring a clean architecture that avoids common pitfalls. This serves as a clear example for developers looking to build scalable and maintainable applications with .NET and MongoDB, fostering robust design from the outset.

## Core Concepts & Architectural Patterns

This project it's a practical guide that showcases several key architectural concepts:

-   **Clean Architecture:** The project is organized into distinct layers, promoting separation of concerns:
    -   `Domain`: Contains the core business entities and repository interfaces.
    -   `Infrastructure`: Handles data access and implementation details (like the MongoDB connection and repository implementation).
    -   `Presentation`: The API layer itself (`Controllers` and `Model` for DTOs), responsible for handling HTTP requests and responses.
-   **Repository Pattern:** Decouples the application logic from the underlying data storage. The `InfectedController` depends on an `IInfectedRepository` interface, not on a concrete MongoDB implementation, making the application more flexible and testable.
-   **Dependency Injection:** All services, from the database connection to the repositories, are managed via .NET's built-in dependency injection framework, promoting loosely coupled and maintainable code.

## Project Structure

```
MongoIntegrationAPI/
├── Controllers/
│   └── InfectedController.cs   // API endpoints
├── Domain/
│   ├── Infected.cs             // Domain entity
│   └── IInfectedRepository.cs  // Repository interface
├── Infrastructure/
│   ├── InfectedRepository.cs   // Repository implementation
│   └── MongoDbSetup.cs         // DI setup for MongoDB
├── Model/
│   └── InfectedDto.cs          // Data Transfer Object
├── appsettings.json            // Configuration
└── Program.cs                  // Application entrypoint and DI setup
```

## Getting Started

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   A running MongoDB instance (local, Docker, or a cloud service like MongoDB Atlas).

### 1. Configuration

Before running the application, you need to configure your MongoDB connection string. Open the `appsettings.json` file and update the `ConnectionString` and `DbName` values.

```json
{
  "ConnectionString": "mongodb://localhost:27017",
  "DbName": "your_database_name",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 2. Running the Application

Navigate to the directory in your terminal and run the following command:

```bash
dotnet run
```
