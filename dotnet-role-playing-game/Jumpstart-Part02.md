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