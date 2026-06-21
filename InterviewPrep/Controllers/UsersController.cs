using InterviewPrep.Models;
using InterviewPrep.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPrep.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        _userService = new UserService(config.GetConnectionString("DefaultConnection"));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _userService.GetUser(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public IActionResult Create([FromBody] User user)
    {
        _userService.CreateUser(user);
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login(string username, string password)
    {
        var result = _userService.Login(username, password);
        if (result)
            return Ok(new { message = "Login successful" });
        return Unauthorized();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.DeleteUser(id);
        return Ok();
    }
}
