namespace Entities;

public class Comment
{
    public int Id { get; set; }
    
    public string Body { get; set; }
    
    public int PostId { get;  }
    
    public int UserId { get; }
    
    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }

    public override string ToString()
    {
        return "Body: " + Body
                        + ", UserId: " + UserId;
    }
}
