namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; }

    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }

    public override string ToString()
    {
        return "ID: "+Id
            +", Title: "+Title
            + ", Body: "+Body;
    }
    
}