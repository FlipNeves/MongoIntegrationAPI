**Clean Architecture in MongoDB \+ C\#: Why is the Repository Pattern Alone Not Enough?**  
*Understanding how mixing Repositories and DAOs solves the dilemma of Embeddings and coupling in .NET.* 

Ever since I started using C\# and wrote my first lines of code, I was trained to apply the Clean Architecture pattern. Following DDD principles, I focused on the Domain layer, keeping it separated from Infrastructure and completely unaware that they represent a database table. 

When I got my first MongoDB project I had several problems. The driver of MongoDB expects some specific methods to search and insert data, instances are built differently and all of this made my first time a traumatic experience. 

It turns out that stepping outside my comfort zone gives me motivation and energy to study and understand things. I started to build projects as Proof of Concepts(POC) trying the connection between C\# .NET and MongoDB in different scenarios. Working on these projects I’ve started to feel something was wrong again, I missed the Generic Repository Pattern and all the ease of dependency injection.

I also noticed another point about MongoDB, because it is a document-oriented database, it structures its “tables” differently. We can make a simple analogy where Tables can be Collections and Rows can be Documents. A document looks a lot like a JSON but it’s not, actually it’s a BSON, is a binary version. The difference between them is the BSON is designed for storage efficiency, it stores metadata such as length and type, because of that it can be parsed and analysed much more quickly by machines than JSON texts. BSON supports basic types such as strings, bools, numbers and also some extra types such as dates, binaries and exclusive identifiers as ObjectIDs.   
*Book:*  
*{*  
*"\_id": { "$oid": "69bc53b77b02eb421c958688" },*   
*"title": "Dune"*  
*}* 

In software we deal with data that is frequently accessed together. In situations like this we can create an Embedded Document. Embedding is basically placing one document or an array of them in a field of another parent document. There are a lot of embedded data patterns such as the Subset Pattern, Extended Reference Pattern and Computed Pattern. All of them fit diverse scenarios. The same book “Dune” but now as an Embedded Document:   
   
Book:  
{  
  "\_id": { "$oid": "69bc53b77b02eb421c958688" },  
  "title": "Dune",  
  "publisherId": { "$oid": "69bc53957b02eb421c958687" },  
  "authors": \[  
    {  
      "name": "Frank Herbert",  
      "bibliography": "\[...\]"  
    }  
  \],  
  "categories": \[  
    {  
      "name": "SciFi",  
      "description": "Science fiction exploring the future and technology."  
    },  
    {  
      "name": "Adventure",  
      "description": "Stories focused on journeys, challenges, and discoveries."  
    }  
  \]  
}

In C\# our class requires specific annotations which, although functional, introduce coupling with the Infrastructure layer. This reduces portability and violates the separation of concerns proposed by Clean Architecture.

Let’s talk more about how we learn to build systems by checking the TreeView of the project.  
*3 folders, no separation between domain and storage*

  *C:\\...\\MongoIntegrationAPI\\*  
  *├───Controllers\\            \<-- Depend directly on the concrete repository classes*  
  *├───Models\\                 \<-- Entities decorated with \[BsonId\], \[BsonRepresentation\], \[BsonElement\]*  
  *├───Repositories\\           \<-- Use IMongoCollection\<\> directly — the MongoDB driver leaks in here*  
  *├───MongoContext.cs         \<-- Exposes IMongoCollection\<T\> to the whole application*  
  *└───MongoSettings.cs        \<-- Connection settings*

Taking a closer look, focusing on a small entity shown in the BSON, the Book. This class Book is a main entity which has its own attributes. For the examples we'll use a simplified version.  

Let’s see how our Book is translated in C\#:  
*using MongoDB.Bson;*  
*using MongoDB.Bson.Serialization.Attributes;*

*public class Book*  
*{*  
    *\[BsonId\]*  
    *\[BsonRepresentation(BsonType.ObjectId)\]*  
    *public string Id { get; set; } \= string.Empty;*

    *\[BsonElement("title")\]*  
    *public string Title { get; set; } \= string.Empty;*

    *\[BsonElement("publisherId")\]*   
    *\[BsonRepresentation(BsonType.ObjectId)\]*  
    *public string PublisherId { get; set; } \= string.Empty;*

    *\[BsonElement("authors")\]*  
    *public List\<AuthorInBook\> Authors { get; set; } \= new();*

    *\[BsonElement("categories")\]*  
    *public List\<Category\> Categories { get; set; } \= new();*  
*}*

*public class AuthorInBook*  
*{*  
    *\[BsonElement("name")\]*  
    *public string Name { get; set; } \= string.Empty;*

    *\[BsonElement("bibliography")\]*  
    *public string Bibliography { get; set; } \= string.Empty;*  
*}*

*public class Category*  
*{*  
    *\[BsonElement("name")\]*  
    *public string Name { get; set; } \= string.Empty;*

    *\[BsonElement("description")\]*  
    *public string Description { get; set; } \= string.Empty;*  
*}*

Adding it in ‘context’:  
*using MongoDB.Driver;*

*public class MongoContext*  
*{*  
*private readonly IMongoDatabase \_database;*

*public MongoContext(MongoSettings settings)*  
*{*  
*var client \= new MongoClient(settings.ConnectionString);*  
*\_database \= client.GetDatabase(settings.DatabaseName);*  
*}*

*public IMongoCollection\<Book\> Books \=\>*  
*\_database.GetCollection\<Book\>("books");*  
*}*

Being used in Repository:  
*using MongoDB.Driver;*

*public class BookRepository*  
*{*  
    *private readonly IMongoCollection\<Book\> \_collection;*

    *public BookRepository(MongoContext context)*  
    *{*  
        *\_collection \= context.Books;*   
    *}*

    *public async Task\<List\<Book\>\> GetAllAsync()*  
    *{*  
        *return await \_collection.Find(\_ \=\> true).ToListAsync();*  
    *}*

    *public async Task\<Book?\> GetByIdAsync(string id)*  
    *{*  
        *return await \_collection.Find(x \=\> x.Id \== id).FirstOrDefaultAsync();*  
    *}*

    *public async Task CreateAsync(Book book)*  
    *{*  
        *await \_collection.InsertOneAsync(book);*  
    *}*  
*}*

Analysing the code it’s possible to identify what is described as “infrastructure infection". Some of the layers on this project are infected by the MongoDB driver. Entities and the repository import it and the controller/service layer depends on these classes. Although the implementation is simple and functional at the first view with our model Book, it introduces some coupling that’s not necessary and it will make the system harder to evolve. 

The entity now depends on MongoDB attributes ObjectId, which means that business rules become tied to persistence details reducing the Domain independence and making changes on drivers or on the storage have an impact in parts of the system and, in theory, they should be stable.

The repository, in turn, fails to fully fulfill its role as an abstraction. Instead of isolating data access, it only wraps direct calls to the MongoDB driver, making the code tightly coupled to the driver’s API. 

 *private readonly IMongoCollection\<Book\> \_collection;*

Repository is not just using the database but MongoDB Driver is shaping it all and bringing the Book class just to satisfy the driver.

It limits all the flexibility of the entire application. This coupling impacts directly on the testability, since the driver’s dependency on interfaces makes creating mocks more complex and, in many cases, leads to an overreliance on integration tests. Can you imagine what could happen if the developer received a task to switch databases or export all entities to a library? It will not be a refactor, it will be a rewrite.

This type of approach, even being common in introductory examples, puts the database as one of the central elements of our software while the domain starts to be in a secondary role. All of this goes against the Clean Architecture principles which propose exactly the opposite: a system driven by behavior and business rules, with the infrastructure being only an external detail.

***\[A Way Out: Translation Layer and Atomic Operations\]***

To guarantee everyone can comprehend how the proposed isolation works and materializes in practice, it’s essential to understand the project’s organization.  The folder structure is not just a style choice but a direct representation of our architecture decisions, establishing clear limits between business rules and persistence details:

  *C:\\...\\MongoIntegrationAPI\\*  
  *├───Domain\\*  
  *│   ├───Entities\\           \<-- Pure POCOs (Author.cs, Book.cs)*  
  *│   ├───Enums\\*  
  *│   └───Interfaces\\         \<-- Contracts (IBookRepository.cs)*  
  *├───Infrastructure\\*  
  *│   ├───DataModels\\         \<-- BSON-decorated models (BookDataModel.cs)*  
  *│   ├───Repositories\\       \<-- Implementations and mappings*  
  *│   ├───Daos\\               \<-- MongoDB-specific operations*  
  *│   └───MongoDbSetup.cs     \<-- Configuration*  
  *└───Controllers\\            \<-- Application entry point*

About the Infrastructure we’re going to have three types of classes: SpecificRepository as BookRepository to receive Domain entities, transform it to DataModels and move to database action. GenericRepository to easy access and reusable methods of CRUD engine, receiving as parameter DataModels and never knowing about Domain. And specific DAO’s as BookDao, precision tool for MongoDB operations in atomic version.

In the directory Domain lies the core of our system, composed exclusively of essential business elements. The entities are defined as simple objects without any dependency on external libraries, while interfaces guarantee clear contracts about what the system needs to do. This layer represents the application’s behavior and must stay isolated from any technical detail. 

In this model, there is no indication of how the data will be persisted in the database. The focus is exclusively on representing the business. 

*//  Domain layer*  
*public class Book*  
*{*  
   *public string Id { get; set; } \= string.Empty;*  
   *public string Title { get; set; } \= string.Empty;*  
   *public string PublisherId { get; set; } \= string.Empty;*  
   *public List\<AuthorInBook\> Authors { get; set; } \= new();*  
   *public List\<Category\> Categories { get; set; } \= new();*  
*}*  
*public class AuthorInBook*  
*{*  
    *public string Id { get; set; } \= string.Empty;*  
    *public string Name { get; set; } \= string.Empty;*  
*}*

You can notice that our entity Book carries embedded information of our Author as the BSON showed earlier but now it’s not a decision of our database, it comes from the rules, the book just needs to show the name of the author. The book entity is loaded, validated and persisted with categories and authors together so we create it on Domain. The difference now is the business operation which made us draw the entity with these attributes and we’re going to see MongoDB just respect this with embedded patterns. If you would keep this shape even with another database, it’s a Domain choice. If not, the database is still in charge.

On the other hand, the infrastructure folder is focusing on all decisions about persistence. This is where DataModels live, responsible for handling specific features of MongoDB. The concrete specific repositories make the bridge between Domain and the Database. This organization ensures the complexity stays contained, preventing it from “leaking” into the rest of the system:

*using MongoDB.Bson;*

*using MongoDB.Bson.Serialization.Attributes;*

*using MongoIntegrationAPI.Infrastructure.Attributes;*

*namespace MongoIntegrationAPI.Infrastructure.DataModels*

*{*

    *// Infrastructure layer: storage decisions*

    *\[CollectionName("books")\]*

    *public class BookDataModel*

    *{*

        *\[BsonId\]*

        *\[BsonRepresentation(BsonType.ObjectId)\]*

        *public string? Id { get; set; }*

        *public string Title { get; set; } \= string.Empty;*

        *\[BsonRepresentation(BsonType.ObjectId)\]*

        *public string PublisherId { get; set; } \= string.Empty;*

        *// The database stores nested objects to optimize reads*

        *public List\<AuthorEmbeddedModel\> Authors { get; set; } \= new List\<AuthorEmbeddedModel\>();*

        *public List\<CategoryEmbeddedModel\> Categories { get; set; } \= new List\<CategoryEmbeddedModel\>();*

    *}*

*}*

DataModel is where we isolate all persistence-related decisions, such as the use of ObjectID and Embeddings structures. This allows us to optimize persistence without affecting the Domain.

Reading our DataModel it’s possible to notice “\[CollectionName(“books”)\]”. It’s not from MongoDB, it’s from our architecture to make it easier to use our Generic Repository pattern. We’ll use MongoDB but how to use just one class to control the commands here. Let’s start looking into how these classes work up to this new attribute in the DataModel.

The interface:

*namespace MongoIntegrationAPI.Infrastructure.Repositories*

*{*

    *public interface IGenericRepository\<TDocument\> where TDocument : class*

    *{*

        *Task AddAsync(TDocument document);*

        *Task\<TDocument?\> GetByIdAsync(string id);*

        *Task\<IEnumerable\<TDocument\>\> GetAllAsync();*

        *Task ReplaceAsync(string id, TDocument document);*

        *Task DeleteAsync(string id);*

    *}*

*}*

The implementation:

*using MongoDB.Bson;*

*using MongoDB.Driver;*

*using MongoIntegrationAPI.Infrastructure.Attributes;*

*using System.Reflection;*

*namespace MongoIntegrationAPI.Infrastructure.Repositories*

*{*

    *public class GenericRepository\<TDocument\> : IGenericRepository\<TDocument\> where TDocument : class*

    *{*

        *private readonly IMongoCollection\<TDocument\> \_collection;*

        *public GenericRepository(IMongoDatabase database)*

        *{*

            *var collectionName \= typeof(TDocument).GetCustomAttribute\<CollectionNameAttribute\>()?.Name*

                *?? typeof(TDocument).Name;*

            *\_collection \= database.GetCollection\<TDocument\>(collectionName);*

        *}*

        *public async Task AddAsync(TDocument document)*

        *{*

            *await \_collection.InsertOneAsync(document);*

        *}*

        *public async Task\<IEnumerable\<TDocument\>\> GetAllAsync()*

        *{*

            *return await \_collection.Find(Builders\<TDocument\>.Filter.Empty).ToListAsync();*

        *}*

        *public async Task\<TDocument?\> GetByIdAsync(string id)*

        *{*

            *var filter \= Builders\<TDocument\>.Filter.Eq("\_id", new ObjectId(id));*

            *return await \_collection.Find(filter).FirstOrDefaultAsync();*

        *}*

        *public async Task ReplaceAsync(string id, TDocument document)*

        *{*

            *var filter \= Builders\<TDocument\>.Filter.Eq("\_id", new ObjectId(id));*

            *await \_collection.ReplaceOneAsync(filter, document);*

        *}*

        *public async Task DeleteAsync(string id)*

        *{*

            *var filter \= Builders\<TDocument\>.Filter.Eq("\_id", new ObjectId(id));*

            *await \_collection.DeleteOneAsync(filter);*

        *}*

    *}*

*}*

The constructor of our Repository is asking a IMongoDatabase to get the Collection found by the ‘collectionName’ which is the result of our new Attribute CollectionNameAttribute on DataModel.

*namespace MongoIntegrationAPI.Infrastructure.Attributes*

*{*

    *\[AttributeUsage(AttributeTargets.Class, Inherited \= false)\]*

    *public class CollectionNameAttribute : Attribute*

    *{*

        *public string Name { get; }*

        *public CollectionNameAttribute(string name)*

        *{*

            *Name \= name;*

        *}*

    *}*

*}*

The main point of this architecture is the specific repository role as translator whenever the Domain and DataModel shapes diverge, which is almost always the case in real systems. It’s no longer a passive intermediary, it explicitly converts the data between the two models and call the generic version to make the updates:

*// Infrastructure*  
*public async Task AddAsync(Book book)*  
*{*  
   *var dataModel \= new BookDataModel*  
   *{*  
       *Title \= book.Title,*  
       *PublisherId \= book.PublisherId,*  
       *Authors \= book.Authors*  
           *.Select(a \=\> new AuthorEmbeddedModel*  
           *{*  
               *Id \= a.Id,*  
               *Name \= a.Name*  
           *})*  
           *.ToList()*  
   *};*

   *await \_genericRepository.AddAsync(dataModel);*  
   *// We adopt the native ObjectId pragmatically, reflecting it back into the domain after creation*  
   *book.Id \= dataModel.Id\!;*  
*}*

It’s here the section that works as a barrier against the corruption of responsibilities, as an anti-corruption layer. The database changes stop here on all these repositories making us able to maintain the stability of our business rules and the generic repository just receives the DataModel objects.

These repositories work well for translating documents. Now let’s analyse performance scenarios in MongoDB. Loading a massive and embedded document into memory just to add a single field in an array is a waste of resources. This is the point where we need a new layer that is responsible for leveraging specific features from MongoDB.

The new layer is going to be the Data Access Object(DAO). Think of it like a hospital. The Repository is the reception desk. You go there to find a patient, schedule an appointment, or retrieve a record. The DAO is the specialist who walks into the archives room, knows exactly which folder to pull, and updates a single page without taking the whole file off the shelf.

*// Infrastructure layer: BookDao.cs (atomic performance)*

*public async Task\<bool\> AddAuthorToBookAtomicAsync(string bookId, AuthorEmbeddedModel author)*

*{*

    *var filter \= Builders\<BookDataModel\>.Filter.Eq(b \=\> b.Id, bookId);*

    *var update \= Builders\<BookDataModel\>.Update.Push(b \=\> b.Authors, author);*

    *var result \= await \_collection.UpdateOneAsync(filter, update);*

    *return result.MatchedCount \> 0;*

*}*

In document database design, there exists a fundamental rule: “data that's accessed together should be stored together”. Use Embeddings to follow the rule and create a denormalization that's very useful to read but can make updates very costly. Coupling the logic on DAO we solve this problem. Instead of loading the entire book into the application then use C\# and writing all the document, we use the operator $push to alter only the necessary fragment directly in the database. The repository orchestrates the business flow, but only the DAO knows the syntax of the specific operations of MongoDB Driver.

The final result is an architecture where Embedding is a clear decision to storage and DAO is a precision tool to manipulate data. In the project it becomes evident that the BookRepository shows a clear method to add an Author, while internally uses the DAO to submit the surgical update of the BSON document. With this we can reach the best of both worlds: a good behavior focused Domain supported by the Infrastructure extracting all the performance MongoDB can offer, all of this works without either side needing to know the other’s technical details.