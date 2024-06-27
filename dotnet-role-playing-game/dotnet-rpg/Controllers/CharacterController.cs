using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;

namespace dotnet_rpg.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterController: ControllerBase
{
    private static readonly Character knight = new Character();

    private static readonly List<Character> characters = [
        new Character(),
        new Character { Id = 1, Name = "Sam"}
    ];
    
    [HttpGet("{id}")]
    public IActionResult GetSingle(int id)
    {
        return Ok(characters.FirstOrDefault(c => c.Id == id));
    }

    [HttpGet("GetAll")]
    public IActionResult GetList()
    {
        return Ok(characters);
    }

    [HttpPost]
    public IActionResult AddCharacter(Character newCharacter)
    {
        characters.Add(newCharacter);
        return Ok(characters);
    }
}
