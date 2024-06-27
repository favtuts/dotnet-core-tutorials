using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;
using dotnet_rpg.Services;

namespace dotnet_rpg.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }    

    [HttpGet("{id}")]
    public IActionResult GetSingle(int id)
    {
        return Ok(_characterService.GetCharacterById(id));
    }

    [HttpGet("GetAll")]
    public IActionResult GetList()
    {
        return Ok(_characterService.GetAllCharacters());
    }

    [HttpPost]
    public IActionResult AddCharacter(Character newCharacter)
    {        
        return Ok(_characterService.AddCharacter(newCharacter));
    }
}
