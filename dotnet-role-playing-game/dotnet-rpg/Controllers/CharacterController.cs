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
        new Character { Name = "Sam"}
    ];
    
    [HttpGet("")]
    public IActionResult GetSingle()
    {
        return Ok(characters[0]);
    }

    [HttpGet("GetAll")]
    public IActionResult GetList()
    {
        return Ok(characters);
    }
}
