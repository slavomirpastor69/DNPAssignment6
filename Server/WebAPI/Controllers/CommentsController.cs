using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    // POST /Comments
    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        ValidateBody(request.Body);
        User user = await userRepo.GetSingleAsync(request.UserId);
        await postRepo.GetSingleAsync(request.PostId);

        Comment comment = new(request.Body, request.PostId, request.UserId);
        Comment created = await commentRepo.AddAsync(comment);
        CommentDto dto = ToDto(created, user.UserName);
        return Created($"/Comments/{dto.Id}", dto);
    }

    // PUT /Comments/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentDto request)
    {
        ValidateBody(request.Body);
        Comment existing = await commentRepo.GetSingleAsync(id);

        existing.Body = request.Body;
        await commentRepo.UpdateAsync(existing);

        return Ok(ToDto(existing, FindUserName(existing.UserId)));
    }

    // GET /Comments/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetComment([FromRoute] int id)
    {
        Comment comment = await commentRepo.GetSingleAsync(id);
        return Ok(ToDto(comment, FindUserName(comment.UserId)));
    }

    // GET /Comments?userId=1&userName=bob&postId=2
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? userId,
        [FromQuery] string? userName,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepo.GetMany();
        Dictionary<int, string> userNames = userRepo.GetMany().ToDictionary(u => u.Id, u => u.UserName);

        if (userId != null)
        {
            comments = comments.Where(c => c.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            comments = comments.Where(c => userNames.ContainsKey(c.UserId)
                                           && userNames[c.UserId].Contains(userName, StringComparison.OrdinalIgnoreCase));
        }

        if (postId != null)
        {
            comments = comments.Where(c => c.PostId == postId);
        }

        return Ok(comments.Select(c => ToDto(c, userNames.GetValueOrDefault(c.UserId))).ToList());
    }

    // DELETE /Comments/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id)
    {
        await commentRepo.DeleteAsync(id);
        return NoContent();
    }

    private string? FindUserName(int userId)
    {
        return userRepo.GetMany().FirstOrDefault(u => u.Id == userId)?.UserName;
    }

    private static void ValidateBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("Comment body cannot be empty.");
        }
    }

    internal static CommentDto ToDto(Comment comment, string? userName)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            UserId = comment.UserId,
            UserName = userName
        };
    }
}
