# How to use EF Core as an In-memory database in ASP.NET Core for testing
* https://tuts.heomi.net/implement-mediator-pattern-with-mediatr-in-csharp/

# Entity Framework Core database providers

```
Install-Package Microsoft.EntityFrameworkCore.InMemory
```

# Create a custom DbContext class in ASP.NET Core

[Database Context Class](./Database/ApiContext.cs)

# Add the Repository service to the services container in ASP.NET Core

```csharp
builder.Services.AddDbContext<ApiContext>(options => options.UseInMemoryDatabase(databaseName: "AuthorDb"));
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
```

# Write and execute unit tests against the in-memory database

[Example unit test: Insert Data Test](EFCoreInMemoryDemoTest.cs)