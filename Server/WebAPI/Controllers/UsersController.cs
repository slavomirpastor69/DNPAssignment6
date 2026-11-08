using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    // POST /Users
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        ValidateUser(request.UserName, request.Password);
        await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new(request.UserName, request.Password);
        User created = await userRepo.AddAsync(user);
        UserDto dto = ToDto(created);
        return Created($"/Users/{dto.Id}", dto);
    }

    // PUT /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        ValidateUser(request.UserName, request.Password);
        User existing = await userRepo.GetSingleAsync(id);

        if (!existing.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase))
        {
            await VerifyUserNameIsAvailableAsync(request.UserName);
        }

        existing.UserName = request.UserName;
        existing.Password = request.Password;
        await userRepo.UpdateAsync(existing);
        return Ok(ToDto(existing));
    }

    // GET /Users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id)
    {
        User user = await userRepo.GetSingleAsync(id);
        return Ok(ToDto(user));
    }

    // GET /Users?userName=bob
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? userName)
    {
        IQueryable<User> users = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            users = users.Where(u => u.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(users.Select(ToDto).ToList());
    }

    // DELETE /Users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await userRepo.DeleteAsync(id);
        return NoContent();
    }

    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        bool taken = await Task.FromResult(userRepo.GetMany()
            .Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)));

        if (taken)
        {
            throw new ArgumentException($"Username '{userName}' is already taken.");
        }
    }

    private static void ValidateUser(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }
    }

    internal static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }
}
