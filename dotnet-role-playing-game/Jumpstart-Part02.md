# Attribute Routing, HTTP Request Methods & Best Practices in .NET Core Web API

# First Steps with Attribute Routing

Add mock list
```csharp
private static readonly List<Character> characters = [
    new Character(),
    new Character { Name = "Sam"}
];
```

Get single endpoint
```csharp
[HttpGet("")]
public IActionResult GetSingle()
{
    return Ok(characters[0]);
}
```

Get all endpoint
```csharp
[HttpGet("GetAll")]
public IActionResult GetList()
{
    return Ok(characters);
}
```

# Routing with Parameters

Add an attribute with the route, the HTTP request method and the parameter. The parameter has to be added in curly brackets.

```csharp
[HttpGet("{id}")]
public IActionResult GetSingle(int id)
{
    return Ok(characters.FirstOrDefault(c => c.Id == id));
}
```

Postman
```
curl --location 'https://localhost:7263/Character/1' \
--header 'accept: */*'
```

# Add a New Character with POST

Add an attribute to this method, and that would be the [HttpPost]
```csharp
[HttpPost]
public IActionResult AddCharacter(Character newCharacter)
{
    characters.Add(newCharacter);
    return Ok(characters);
}
```

Postman
```
curl --location 'https://localhost:7263/Character' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
    "id" : 3,
    "name" : "Percival"
}'
```

# Best Practice: Web API Structure

We will inject the necessary services into the controller - so we will use dependency injection.

We can also introduce the idea of Data Transfer Objects, or short DTOs. We already have Models, but it’s common to use these DTOs for the communication between client and server.

The difference is this: DTOs are objects you won’t find in the database, i.e. they won’t be mapped. Models, in turn, are a representation of a database table.

In this case we want to save this information in the database but don’t want to send it back to the client. Here the DTO comes in.

We grab the model and map information of the model to the DTO. There are libraries that do this for us, like Automapper, so we don’t have to do this manually.

Apart from that we can also create DTOs that combine properties of several models. They simply give us more freedom in what we want to return to the client.

# Character Service

To format code: Press CTRL + K + F , or right click -> Format Selection

To add using directives for the missing types, click the lightbulb or press CTRL + . (control and period)

To add a constructor, We can add it with the snippet `ctor`. So, just write `ctor` and then hit `tab`.


