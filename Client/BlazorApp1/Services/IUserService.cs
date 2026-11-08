using ApiContracts;

namespace BlazorApp1.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
}
