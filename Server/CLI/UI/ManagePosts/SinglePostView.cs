using Entities;
using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly PostFileRepository postRepository;
    private readonly CommentFileRepository commentRepository;
    
    public  SinglePostView(PostFileRepository postRepository, CommentFileRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowPost()
    {
        Console.WriteLine("Which post you want to view?");

        int postID = int.Parse(Console.ReadLine());

        Post post = await postRepository.GetSingleAsync(postID);

        IQueryable<Comment> postComments =
            commentRepository.GetManyFromPost(postID);

        List<Comment> comments =
            new List<Comment>(postComments);

        Console.WriteLine(post);

        for (int i = 0; i < comments.Count; i++)
        {
            Console.WriteLine(comments[i]);
        }
    }
}