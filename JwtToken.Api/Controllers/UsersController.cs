using JwtToken.Api.DTOs;
using JwtToken.Api.Helpers;
using JwtToken.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtToken.Api.Controllers;

[Route("api/[controller]")] //    /api/Users
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = await userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var user = await userService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}