using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly ICommentRepository commentRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }

    // POST /Posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        ValidatePost(request.Title, request.Body);
        User author = await userRepo.GetSingleAsync(request.UserId);

        Post post = new(request.Title, request.Body, request.UserId);
        Post created = await postRepo.AddAsync(post);
        PostDto dto = ToDto(created, author.UserName);
        return Created($"/Posts/{dto.Id}", dto);
    }

    // PUT /Posts/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostDto>> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
    {
        ValidatePost(request.Title, request.Body);
        Post existing = await postRepo.GetSingleAsync(id);

        existing.Title = request.Title;
        existing.Body = request.Body;
        await postRepo.UpdateAsync(existing);

        return Ok(ToDto(existing, FindUserName(existing.UserId)));
    }

    // GET /Posts/{id}  - includes author name and comments
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPost([FromRoute] int id)
    {
        Post post = await postRepo.GetSingleAsync(id);
        Dictionary<int, string> userNames = GetUserNames();

        PostDto dto = ToDto(post, userNames.GetValueOrDefault(post.UserId));
        dto.Comments = commentRepo.GetManyFromPost(id).OrderBy(c => c.Id)
            .Select(c => CommentsController.ToDto(c, userNames.GetValueOrDefault(c.UserId)))
            .ToList();

        return Ok(dto);
    }

    // GET /Posts?title=abc&userId=1&userName=bob
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts(
        [FromQuery] string? title,
        [FromQuery] int? userId,
        [FromQuery] string? userName)
    {
        IQueryable<Post> posts = postRepo.GetMany();
        Dictionary<int, string> userNames = GetUserNames();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (userId != null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            posts = posts.Where(p => userNames.ContainsKey(p.UserId)
                                     && userNames[p.UserId].Contains(userName, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(posts.Select(p => ToDto(p, userNames.GetValueOrDefault(p.UserId))).ToList());
    }

    // GET /Posts/{id}/Comments  - nested route for the post's comments
    [HttpGet("{id:int}/Comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsForPost([FromRoute] int id)
    {
        await postRepo.GetSingleAsync(id); // throws NotFoundException if the post does not exist
        Dictionary<int, string> userNames = GetUserNames();

        List<CommentDto> comments = commentRepo.GetManyFromPost(id).OrderBy(c => c.Id)
            .Select(c => CommentsController.ToDto(c, userNames.GetValueOrDefault(c.UserId)))
            .ToList();

        return Ok(comments);
    }

    // DELETE /Posts/{id}  - also deletes the post's comments
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost([FromRoute] int id)
    {
        await postRepo.DeleteAsync(id);

        List<int> commentIds = commentRepo.GetManyFromPost(id).Select(c => c.Id).ToList();
        foreach (int commentId in commentIds)
        {
            await commentRepo.DeleteAsync(commentId);
        }

        return NoContent();
    }

    private Dictionary<int, string> GetUserNames()
    {
        return userRepo.GetMany().ToDictionary(u => u.Id, u => u.UserName);
    }

    private string? FindUserName(int userId)
    {
        return userRepo.GetMany().FirstOrDefault(u => u.Id == userId)?.UserName;
    }

    private static void ValidatePost(string title, string body)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException("Body cannot be empty.");
        }
    }

    private static PostDto ToDto(Post post, string? authorName)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            AuthorName = authorName
        };
    }
}
