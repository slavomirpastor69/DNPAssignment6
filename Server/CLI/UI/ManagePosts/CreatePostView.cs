using RepositoryContracts;
using Entities;
using FileRepositories;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly PostFileRepository postRepository;

    public CreatePostView(PostFileRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void CreatePost()
    {
        Console.Write("What is your user id?: ");
        int userId = int.Parse(Console.ReadLine());
        Console.Write("Title: "); 
        String title = Console.ReadLine();
        Console.Write("Body: "); 
        String body = Console.ReadLine();
        Post post = new Post(title, body, userId);
        postRepository.AddAsync(post);
    }
}