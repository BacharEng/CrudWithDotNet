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

        //To check if user exists before creating we do:
        //1. getUserByEmail - input email, output bool
        //2. if email exists - throw with message email already exists
        //3. if email returns null/empty - continue to create

        await _mongoDBServices.CreateNewUser(userToSave);
        return CreatedAtAction(nameof(GetUserById), new { id = userToSave.Id }, userToSave);
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUserAsync(string id)
    {
        var existingUser = await _mongoDBServices.GetUserAsync(id);
        if (existingUser == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        await _mongoDBServices.DeleteUserAsync(id);
        return Ok($"User with ID {id} deleted successfully");
    }

    [HttpPut("UpdateUser/{id}")]
    public async Task<ActionResult> UpdateUser(string id, [FromBody] User updateUser)
    {
        var existingUser = await _mongoDBServices.GetUserAsync(id);
         if (existingUser == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        existingUser.firstName = updateUser.firstName;
        existingUser.lastName = updateUser.lastName;
        existingUser.age = updateUser.age;

        if(updateUser.password != null)
        {
            var hash = _authService.HashPassword(updateUser.password);
            existingUser.password = hash;
        }

        await _mongoDBServices.UpdateUserAsync(existingUser);
        return Ok($"User with ID {id} updated successfully");
        

    }

    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _mongoDBServices.GetUserAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found");
        }

        return user;
    }


}