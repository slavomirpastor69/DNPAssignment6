using System.Text.Json;

namespace FileRepositories;
using Entities;
using RepositoryContracts;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment>? comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);

        comments ??= new List<Comment>();
        
        Comment? existingComment =
            comments.SingleOrDefault(c => c.Id == comment.Id);

        if (existingComment == null)
        {
            throw new NotFoundException($"Comment with ID {comment.Id} was not found.");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        commentsAsJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment>? comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);

        comments ??= new List<Comment>();
        
        Comment? existingComment =
            comments.SingleOrDefault(c => c.Id == id);

        if (existingComment == null)
        {
            throw new NotFoundException($"Comment with ID {id} was not found.");
        }

        comments.Remove(existingComment);
        
        commentsAsJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment>? comments =
            JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);

        comments ??= new List<Comment>();
        
        Comment? existingComment =
            comments.SingleOrDefault(c => c.Id == id);

        if (existingComment == null)
        {
            throw new NotFoundException($"Comment with ID {id} was not found.");
        }

        return existingComment;
    }

    public IQueryable<Comment> GetMany() {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }

    public IQueryable<Comment> GetManyFromPost(int id)
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;

        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        comments ??= new List<Comment>();

        List<Comment> postComments = comments.Where(c => c.PostId == id).ToList();
        
        return postComments.AsQueryable();
    }
}