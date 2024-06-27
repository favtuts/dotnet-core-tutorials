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

And you can access the SwaggerUI:
```
https://localhost:7263/swagger/index.html
```

You can test the `weatherforecast` default API
```
curl --location 'https://localhost:7263/weatherforecast' \
--header 'accept: application/json'
```

# New Models

First, we create a "Models" folder. So right-click the Models folder, then click “New C# Class”.
* [Character class](./dotnet-rpg/Models/Character.cs)

We will also add an RpgClass property, i.e. the type of the character. But first, we have to create a new enum for that.
* [RpgClass](./dotnet-rpg/Models/RpgClass.cs)

# New Controller

To add a new controller, we create a new C# class in the Controllers folder. Let’s call this class CharacterController.
* [CharacterController class](./dotnet-rpg/Controllers/CharacterController.cs)

You can test with Postman
```
curl --location 'https://localhost:7263/Character' \
--header 'accept: */*'
```