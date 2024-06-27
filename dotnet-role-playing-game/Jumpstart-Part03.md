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


# Proper Service Response with Generics

Create a new class and call the file `ServiceResponse<T>`. This is the Generic class for returing a wrapper object to client with every service call. Advantages are that you can add additional information to the returning result, like a success or exception message. The front end is able to react to this additional information and read the actual data with the help of HTTP interceptors

```csharp
public class ServiceResponse<T>
{
    public T Data { get; set; }

    public bool Success { get; set; } = true;

    public string Message { get; set; } = null;
}
```

The `Data` of type `T` is, well, the actual data like the RPG characters. With the `Success` property we can tell the front end if everything went right, and the `Message` property can be used to send a nice explanatory message, e.g. in case of an error.

To make use of our new `ServiceResponse`, we have to modify the return types of our `CharacterService` and `ICharacterService` methods.
```csharp
public interface ICharacterService
{
    Task<ServiceResponse<List<Character>>> GetAllCharacters();
    Task<ServiceResponse<Character>> GetCharacterById(int id);
    Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter);
}
```