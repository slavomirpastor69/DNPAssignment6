using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    // POST /Auth/login
    [HttpPost("login")]
    public ActionResult<UserDto> Login([FromBody] LoginRequest request)
    {
        // Usernames are unique regardless of casing, see UsersController.
        User? user = userRepo.GetMany()
            .FirstOrDefault(u => u.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase));

        // The same message for both cases, so the response does not reveal which usernames exist.
        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Incorrect username or password.");
        }

        return Ok(UsersController.ToDto(user));
    }
}
