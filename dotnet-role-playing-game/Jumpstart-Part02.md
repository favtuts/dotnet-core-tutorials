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