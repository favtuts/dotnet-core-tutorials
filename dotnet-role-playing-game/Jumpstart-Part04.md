# Update & Remove Entities in .NET Core Web API with PUT & DELETE

# Modify a Character with PUT

To modify or update an RPG character, we have to add a new method to the `ICharacterService` interface, the `CharacterService` class and the `CharacterController`. Let’s start with the interface and add the method `UpdateCharacter`. The return type is a `GetCharacterDto` so that we can see the changes directly, and the parameter is a new DTO, `UpdateCharacterDto`.

```csharp
public interface ICharacterService
{    
    Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);
}
```

We create the new C# class `UpdateCharacterDto` and actually can copy and paste all properties from the `GetCharacterDto`.
```csharp
public class UpdateCharacterDto
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

Now we can implement the method in the `CharacterService`
```csharp
public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
{
    ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

    Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
    character.Name = updatedCharacter.Name;
    character.Class = updatedCharacter.Class;
    character.Defense = updatedCharacter.Defense;
    character.HitPoints = updatedCharacter.HitPoints;
    character.Intelligence = updatedCharacter.Intelligence;
    character.Strength = updatedCharacter.Strength;

    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);

    return serviceResponse;
}
```

Okay, off to the `CharacterController`. In essence, we can copy the `AddCharacter()` method, change the name, the service method, and the parameter, and also the HTTP method attribute, which is `[HttpPut]`.
```csharp
[HttpPut]
public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updatedCharacter)
{
    return Ok(await _characterService.UpdateCharacter(updatedCharacter));
}
```

Let's test it with Postman
```
curl --location --request PUT 'https://localhost:7263/Character' \
--header 'accept: */*' \
--header 'Content-Type: application/json' \
--data '{
    "id" : 1,
    "name" : "Percival",
    "hitpoints" : 200,
    "class": 2
}'
```

Now, let’s try to update a character that doesn’t exist. Just change the id to 2 and see what happens. We’re getting a `NullReferenceException`
```
System.NullReferenceException: Object reference not set to an instance of an object.
   at dotnet_rpg.Services.CharacterService.UpdateCharacter(UpdateCharacterDto updatedCharacter)
```
We have two options now. 
* We catch that exception with a try/catch block
* We just check if we find a character in the characters list.

Let’s start with the try/catch block
```csharp
public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
{
    ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
    try
    {
        Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);
        character.Name = updatedCharacter.Name;
        character.Class = updatedCharacter.Class;
        character.Defense = updatedCharacter.Defense;
        character.HitPoints = updatedCharacter.HitPoints;
        character.Intelligence = updatedCharacter.Intelligence;
        character.Strength = updatedCharacter.Strength;

        serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
    }
    catch (Exception ex)
    {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
    }
    return serviceResponse;
}
```

That’s it already. Let’s test that again in Postman.
```json
{
    "data": null,
    "success": false,
    "message": "Object reference not set to an instance of an object."
}
```

A possible front end can work with that. Maybe another message would be more suitable for the user.

Alternatively, we can add a slight modification to the `CharacterController`. You see, that we’re still getting a status code `200 OK`. Well, a character wasn’t found, so maybe we can also return a `404 Not Found` response.
```csharp
[HttpPut]
public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updatedCharacter)
{
    ServiceResponse<GetCharacterDto> response = await _characterService.UpdateCharacter(updatedCharacter);
    if (response.Data == null)
    {
       return NotFound(response);
    }

    return Ok(response);
}
```

When we test that now in Postman, we’re still getting the whole response back, but the status code is 404 Not Found. Feel free to play around with that. For instance, you do not have to use the Message of the ServiceResponse only in case of an error. What about a success message like “You’re character has been saved.”?

# Delete a Character

To delete an RPG character, again, we have to make modifications to the `ICharacterService` interface, the `CharacterService` and the `CharacterController`.

```csharp
ask<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
```

Regarding the `CharacterService`, we can implement the interface automatically again and then copy the code of the `UpdateCharacter()` method and just make some changes.
```csharp
public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
{
    ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
    try
    {
        Character character = characters.First(c => c.Id == id);
        characters.Remove(character);

        serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
    }
    catch (Exception ex)
    {
        serviceResponse.Success = false;
        serviceResponse.Message = ex.Message;
    }
    return serviceResponse;
}
```

The controller method is a combination of the` GetSingle()` and `UpdateCharacter()` method.
```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    ServiceResponse<List<GetCharacterDto>> response = await _characterService.DeleteCharacter(id);
    if (response.Data == null)
    {
        return NotFound(response);
    }

    return Ok(response);
}
```

Let's test it with PostMan
```
curl --location --request DELETE 'https://localhost:7263/Character/1' \
--header 'accept: */*'
```