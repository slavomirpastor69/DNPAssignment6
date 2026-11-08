using CLI.UI.ManagePosts;
using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentView
{
    private readonly CommentFileRepository commentRepository;
    private CreateCommentView createCommentView;

    public ManageCommentView(CommentFileRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public void Options()
    {
        Console.WriteLine("1. Create comment");
        String?  choice = Console.ReadLine();
        switch (choice)
        {
            case null : 
                Console.WriteLine("That option doesnt exist");
                break;
            case "1"  :
                createCommentView = new CreateCommentView(commentRepository);
                createCommentView.postComment();
                break;
        }
    }
}