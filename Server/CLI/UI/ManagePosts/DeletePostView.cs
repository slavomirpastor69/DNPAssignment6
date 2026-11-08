using Entities;
using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class DeletePostView
{
    private readonly PostFileRepository postRepository;
    private readonly CommentFileRepository commentRepository; 
    
    public DeletePostView (PostFileRepository postRepository, CommentFileRepository
         commentRepository)
        {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        }

    public async void deletePost()
    {
        Console.Write("Which Post you want to delete?: ");
        int postID = int.Parse(Console.ReadLine());
        
        postRepository.DeleteAsync(postID);
    }
}