using ApiContracts;

namespace BlazorApp1.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("comments", request);
        return await ApiResponse.ReadAsync<CommentDto>(response);
    }
}
