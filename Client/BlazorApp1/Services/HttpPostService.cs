using ApiContracts;

namespace BlazorApp1.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("posts", request);
        return await ApiResponse.ReadAsync<PostDto>(response);
    }

    public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"posts/{id}", request);
        return await ApiResponse.ReadAsync<PostDto>(response);
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"posts/{id}");
        return await ApiResponse.ReadAsync<PostDto>(response);
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        HttpResponseMessage response = await client.GetAsync("posts");
        return await ApiResponse.ReadAsync<List<PostDto>>(response);
    }
}
