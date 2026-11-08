using Entities;
using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView(PostFileRepository postRepository)
{
    private readonly PostFileRepository postRepository = postRepository;

    public async Task ShowAllPosts()
    {
        List<Post> posts =
            postRepository.GetMany().ToList(); 
        for (int i = 0; i < posts.Count; i++)
        {
            Console.WriteLine(posts[i]);
        }
    }
    
}