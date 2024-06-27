using Microsoft.AspNetCore.Mvc;
using dotnet_rpg.Models;

namespace dotnet_rpg.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterController: ControllerBase
{
    private static Character knight = new Character();
    
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok(knight);
    }    
}
