# Object-Relational Mapping & Code First Migration with Entity Framework Core

# Installing Entity Framework Core

We’re going to use SQL Server, so we need to install the Entity Framework Core Package for SQL Server. You can find it on `nuget.org` or you open a terminal window and enter:
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

After the installation of that package, you’ll find the reference in the project file.
```
  <ItemGroup>
    <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="7.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.1" />
  </ItemGroup>
```

The next step would be installing the actual Entity Framework tool that allows us to use Entity Framework migrations.
```
dotnet tool install --global dotnet-ef
```

Then we have to add another package:
```
dotnet add package Microsoft.EntityFrameworkCore.Design
```
By the way, this was not necessary with previous versions of .NET Core. Now, these installations enable the `dotnet ef` commands on a specific project. Entity Framework Core is now available for our project.

# Installing SQL Server Express (with Management Studio)

For this project, we install two more things, SQL Server Express and SQL Server Management Studio to have a look at our actual database.

The easiest way to get SQL Server Express would be to simply google for it. The first hit should already take you to the [current SQL Server version (2019)](https://www.microsoft.com/en-us/sql-server/sql-server-2019).

