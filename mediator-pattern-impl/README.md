# Implement Mediator Pattern with MediatR in C#
* https://tuts.heomi.net/implement-mediator-pattern-with-mediatr-in-csharp/
* https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli


# Create projects and install required package

Create `MediatRAPI` which will server Customer API
```
dotnet new webapi -o MediatRAPI
```

Create another class library called `MediatRHandler`s where we configure requests and request handlers
```
dotnet new classlib -o MediatRHandler
```

Create a Solution for 2 projects
```
dotnet new sln -n MediatRDemo
dotnet sln add MediatRAPI/MediatRAPI.csproj
dotnet sln add MediatRHandler/MediatRHandler.csproj
dotnet build
```

Add dependencies for `MediatRHandler` project
```
cd MediatRHandler/
dotnet add package MediatR
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
dotnet add package Microsoft.Extensions.Logging
```


Add dependencies for `MediatRAPI`
```
cd ../
cd MediatRAPI/
dotnet add reference ../MediatRHandler/MediatRHandler.csproj
dotnet add package MediatR
dotnet add package Serilog.AspNetCore
```

Try to build the solution
```
cd ../
dotnet build
```


# Configuration Database

In the `appsettings.json` file we add a new string, like `SqliteConnection`, and set it to `Data Source=mediatrdemo.db`. 

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\v11.0;Database=mediatrdemo;Trusted_Connection=True;MultipleActiveResultSets=true",
    "SqliteConnection": "Data Source=mediatrdemo.sqlite"
},
```


# Create Model

First, create the [Customer class](./MediatRHandler/Entitites/Customer.cs)

# Create DBContext

Then create [Database context class](./MediatRHandler/Data/AppDbContext.cs)

# Install DOTNET EF tool
* https://stackoverflow.com/questions/57066856/command-dotnet-ef-not-found

To install the `dotnet-e`f` tool, run the following command:

.NET 8
```
dotnet tool install --global dotnet-ef --version 8.*
```

This work on VS Code Ubuntu
```
$ dotnet tool install --global dotnet-ef
Skipping NuGet package signature verification.
You can invoke the tool using the following command: dotnet-ef
Tool 'dotnet-ef' (version '8.0.6') was successfully installed.
```
then
```
$ dotnet tool restore
```

After that all the execution are done like
```
dotnet tool run dotnet-ef
```
or
```
dotnet dotnet-ef
```

If you're using Snap package `dotnet-sdk` on `Linux`, this can be resolved by updating your `~.bashrc` file / etc. as follows:
```bash
#!/bin/bash
export DOTNET_ROOT=/snap/dotnet-sdk/current
export MSBuildSDKsPath=$DOTNET_ROOT/sdk/$(${DOTNET_ROOT}/dotnet --version)/Sdks
export PATH="${PATH}:${DOTNET_ROOT}"
export PATH="$PATH:$HOME/.dotnet/tools"
```

# Create the SQLite Database

First, you have removed the folder with all migration files
```
cd MediatRAPI/
```

Then add a migration again with dotnet ef migrations add Initial
```
$ dotnet ef migrations add Initial

Your startup project 'MediatRAPI' doesn't reference Microsoft.EntityFrameworkCore.Design. This package is required for the Entity Framework Core Tools to work. Ensure your startup project is correct, install the package, and try again.
```

Install the missing package and run migrations again
```
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

You may have the error, and here is fix:
* https://stackoverflow.com/questions/59265606/your-target-project-x-doesnt-match-your-migrations-assembly-xx-either-chan

Change target assembly from MediatRAPI
```
$ cd ../MediatRHandler
$ dotnet ef --startup-project ../MediatRAPI/ migrations add Initial
Build started...
Build succeeded.
[10:08:47 INF] Starting web host
[10:08:47 INF] Data services registered
[10:08:47 INF] MediatR services registered
Done. To undo this action, use 'ef migrations remove'
```

Final, we create the database file with dotnet ef database update.
```
$ dotnet ef --startup-project ../MediatRAPI/ database update
```

#  Create Requests

In the MediatRHandler, let us create two requests
* [Request for creating a Customer](./MediatRHandler/Requests/CreateCustomerRequest.cs)
* [Request for retrieving the Customer by Customer Id](./MediatRHandler/Requests/GetCustomerRequest.cs)


# Create Handlers

For each of the above requests create handlers as shown below:
* [Handler for creating customer](./MediatRHandler/RequestHandlers/CreateCustomerHandler.cs)
* [Handler for getting customer](./MediatRHandler/RequestHandlers/GetCustomerHandler.cs)

# Create Controllers

Create a [Customer Controller](./MediatRAPI/Controllers/CustomerController.cs). We are not injecting all the handlers instead we are injecting only the mediator.

# Wire up the registrations

* [Register for data services](./MediatRHandler/MediatRDependencyHandler.cs)
* [Register for MediatR handlers](./MediatRHandler/MediatRDependencyHandler.cs)

Starting up
```csharp
...

// Add services to the container.
builder.Services.RegisterDatabaseServices(builder.Configuration, microsoftLogger);
builder.Services.RegisterRequestHandlers(microsoftLogger);

// Add controllers to the container
builder.Services.AddControllers();

...

app.MapControllers();

app.Run();
```