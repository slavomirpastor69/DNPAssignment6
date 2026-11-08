using RepositoryContracts;
using Entities;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    
    public  CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public void postComment()
    {
        Console.Write("Under which post you want to comment?: ");
        int postID = int.Parse(Console.ReadLine());
        Console.Write("Who are you (UserID): ");
        int userID = int.Parse(Console.ReadLine());
        Console.Write("New Comment: ");
        String? body = Console.ReadLine();
        Comment comment = new Comment(body, postID, userID );

        commentRepository.AddAsync(comment);

    }
}