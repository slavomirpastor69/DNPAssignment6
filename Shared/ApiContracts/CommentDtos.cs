namespace ApiContracts;

public class CreateCommentDto
{
    public required string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
}

public class UpdateCommentDto
{
    public required string Body { get; set; }
}

public class CommentDto
{
    public int Id { get; set; }
    public required string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
}
