using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        posts ??= [];

        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        
        if (existingPost == null)
        {
            throw new NotFoundException($"Post with ID {post.Id} was not found.");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        postsAsJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        posts ??= [];

        Post? existingPost = posts.SingleOrDefault(p => p.Id == id);
        
        if (existingPost == null)
        {
            throw new NotFoundException($"Post with ID {id} was not found.");
        }

        posts.Remove(existingPost);
        
        postsAsJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        posts ??= [];

        Post? existingPost = posts.SingleOrDefault(p => p.Id == id);
        
        if (existingPost == null)
        {
            throw new NotFoundException($"Post with ID {id} was not found.");
        }

        return existingPost;
    }

    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        posts ??= [];

        return posts.AsQueryable();
    }
    
}