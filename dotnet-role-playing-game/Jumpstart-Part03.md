# Asynchronous Calls, Data-Transfer-Objects & Automapper in .NET Core Web API

# Asynchronous Calls

Here we just add the `Task` type to our return types.
```csharp
public interface ICharacterService
{
    Task<List<Character>> GetAllCharacters();
    Task<Character> GetCharacterById(int id);
    Task<List<Character>> AddCharacter(Character newCharacter);
}
```

Additionally we add the word `async` to the methods.
```csharp
public async Task<List<Character>> AddCharacter(Character newCharacter)
{
    characters.Add(newCharacter);
    return characters;
}

public async Task<List<Character>> GetAllCharacters()
{
    return characters;
}

public async Task<Character> GetCharacterById(int id)
{
    return characters.FirstOrDefault(c => c.Id == id);
}
```

In thhe `CharacterController`. Again we add the `Task` type with the corresponding using directive and the `async` keyword to every method. Additionally we add the keyword `await` to the actual service call.