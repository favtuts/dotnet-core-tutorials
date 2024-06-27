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

# Data-Transfer-Objects (DTOs)

First things first, let’s create a folder called `Dtos` and then another folder called `Character` for all the data transfer objects regarding the RPG characters.

As already mentioned, the idea behind DTOs is that you’ve got smaller objects that do not consist of every property of the corresponding model. When we create a database table for our RPG characters later in this tutorial series, we will add properties like the created and modified date or a flag for the soft deletion of that character. We don’t want to send this data to the client.

At the moment, we have these to cases: Receiving RPG characters from the server and sending a new character to the server. So let’s create two classes called `GetCharacterDto` and `AddCharacterDto`.

Regarding the `GetCharacterDto`, it should look exactly the same as the Character model for now
```csharp
using dotnet_rpg.Models;

namespace dotnet_rpg.Dtos.Character;

public class GetCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Frodo";
    public int HitPoints { get; set; } = 100;
    public int Strength { get; set; } = 10;
    public int Defense { get; set; } = 10;
    public int Intelligence { get; set; } = 10;
    public RpgClass Class { get; set; } = RpgClass.Knight;
}
```

The `AddCharacterDto` looks a bit different. Let’s say we want to send every property but the Id to the service. So we can copy the properties of the model again, but remove the `Id`.

We start with the `ICharacterService` interface. Instead of the `Character` type we now return `GetCharacterDto` . The parameter of the `AddCharacter()` method now is of type `AddCharacterDto`. We also have to add the corresponding using directive.

```csharp
public interface ICharacterService
{
    Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
    Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
    Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
}
```

We have to map the DTOs with the model. It’s time for `AutoMapper`. To install the package, we go to the terminal and enter bellow command without any specific version to install the latest package.
```
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

After the installation, we register AutoMapper:
```csharp
builder.Services.AddAutoMapper(typeof(Program));
```

To be able to use the mapping of `AutoMapper`, we now need an instance of the mapper in our service. So in the `CharacterService` we add a constructor and inject `IMapper`. Again, we can initialize a field from that parameter and add an underscore to the private field.
```csharp
private readonly IMapper _mapper;

public CharacterService(IMapper mapper)
{
    _mapper = mapper;
}
```

Now we can use the `_mapper` to set the correct types to the `Data` property of our `ServiceResponse`.

Let’s start with the `GetCharacterById()` method. 
```csharp
public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
{
    ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
    serviceResponse.Data = _mapper.Map<GetCharacterDto>(characters.FirstOrDefault(c => c.Id == id));
    return serviceResponse;
}
```

In the `AddCharacter()` method we first map the `newCharacter` into the `Character` type, because it will be added to the characters list. So, it’s the other way around.
```csharp
public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
{
    ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

    characters.Add(_mapper.Map<Character>(newCharacter));
    serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
    return serviceResponse;
}
```

And finally in the `GetAllCharacters()` method, we map every single RPG character of the characters list with `Select()` again, similar to the `AddCharacter()` method.
```csharp
public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
{
    ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
    serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
    return serviceResponse;
}
```

Let’s test this now with Postman, Let’s try it with the GetAll route (http://localhost:5000/Character/GetAll), and we’re getting an error.
```bash
AutoMapper.AutoMapperMappingException: Missing type map configuration or unsupported mapping.

Mapping types:
Character -> GetCharacterDto
```

AutoMapper does not know how to map `Character` into a `GetCharacterDto`. You might ask yourself, “it’s called AutoMapper, so why isn’t this working automatically?”

We have to create maps for the mappings, and this is organized in `Profiles`. You could create a `Profile` for every mapping, but let’s spare the hassle and just create one class for all profiles for now.

At root level, we can create a new C# class and call it `AutoMapperProfile`. This class derives from `Profile`. Make sure to add the `AutoMapper` using directive.

```csharp
public AutoMapperProfile()
{
    CreateMap<Character, GetCharacterDto>();            
    CreateMap<AddCharacterDto, Character>();
}
```

When we test this now, you see that everything works, but the `id` is `0`. That’s because the `AddCharacterDto` does not provide an `Id`. That’s exactly what we wanted. Still, let’s fix this by generating a proper Id ourselves.


In the `AddCharacter()` method in the `CharacterService` we first create our new `Character` based on the DTO and then set the correct `Id` by finding the current max value in the characters list and increasing it by 1.
```csharp
public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
{
    ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
    Character character = _mapper.Map<Character>(newCharacter);
    character.Id = characters.Max(c => c.Id) + 1;
    characters.Add(character);
    serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
    return serviceResponse;
}
```