using ApiContracts;

namespace BlazorApp1.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> UpdatePostAsync(int id, UpdatePostDto request);
    Task<PostDto> GetPostAsync(int id);
    Task<List<PostDto>> GetPostsAsync();
}
