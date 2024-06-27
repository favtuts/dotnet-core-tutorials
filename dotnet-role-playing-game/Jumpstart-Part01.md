# .NET Core Web API & Entity Framework Jumpstart - Part 1

# Create a new Web API

First list all the available templates
```
dotnet new list
dotnet new list web
```

We create new WebAPI project.
```
dotnet new webapi
```

The Visual Studio Code C# extension can generate assets to build and debug for you. If you missed the prompt when you first opened a new C# project, you can still run this command by opening the Command Palette (`View > Command Palette`) and typing "`>.NET: Generate Assets for Build and Debug`". Selecting this will generate the .vscode, launch.json, and tasks.json configuration files that you need.

You should see now, that we got the `.vscode` folder with the `launch.json` and the `tasks.json`.

# First API Call

In the terminal, we enter dotnet run. 
```
dotnet run
```