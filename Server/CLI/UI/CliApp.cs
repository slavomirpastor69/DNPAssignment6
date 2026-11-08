using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using FileRepositories;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly PostFileRepository postRepository;
    private readonly CommentFileRepository commentRepository;
    private readonly UserFileRepository userRepository;
    private ManagePostsView managePostsView;
    private ManageUsersView manageUsersView;
    private ManageCommentView manageCommentView;
    
    public CliApp(UserFileRepository userRepository,
        CommentFileRepository commentRepository, PostFileRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        
    }

    public async Task StartAsync()
    {
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("1. Manage Posts");
        Console.WriteLine("2. Manage Comments");
        Console.WriteLine("3. Manage Users");
        String? choice = Console.ReadLine();

        switch (choice)
        {
            case null  : Console.WriteLine("No valid option");
                break;
            case "1":
                managePostsView = new ManagePostsView(postRepository, commentRepository);
                managePostsView.Options();
                break;
            case "2" : 
                manageCommentView = new ManageCommentView(commentRepository);
                manageCommentView.Options();
                break;
            case "3" : 
                manageUsersView = new ManageUsersView(userRepository);
                manageUsersView.Options();
                break;
        }
    }
}