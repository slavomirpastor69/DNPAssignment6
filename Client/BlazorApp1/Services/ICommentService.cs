using ApiContracts;

namespace BlazorApp1.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
}
