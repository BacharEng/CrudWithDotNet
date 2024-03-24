using Microsoft.AspNetCore.Mvc;
using CrudWithDotNet.Models;
using CrudWithDotNet.Services;

namespace CrudWithDotNet.Controllers;


[Controller]
[Route("api/[controller]")]

public class UserController: Controller 
{
    private readonly MongoDBServices _mongoDBServices;
    private readonly AuthService _authService;
    public UserController(MongoDBServices mongoDBServices, AuthService authService)
    {
        _authService = authService;
        _mongoDBServices = mongoDBServices;
    }

    [HttpGet("GetAllUsers")]
    public async Task<List<User>> GetAllUsers()
    {
        return await _mongoDBServices.GetUsersAsync();
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        var hash = _authService.HashPassword(user.password);

        var userToSave = new User
        {
            firstName = user.firstName,
            lastName = user.lastName,
            email = user.email,
            password = hash,
        };

        await _mongoDBServices.CreateNewUser(userToSave);
        return CreatedAtAction(nameof(GetUserById), new { id = userToSave.Id }, userToSave);
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _mongoDBServices.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }


}