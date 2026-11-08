using ApiContracts;

namespace BlazorApp1.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("users", request);
        return await ApiResponse.ReadAsync<UserDto>(response);
    }
}
