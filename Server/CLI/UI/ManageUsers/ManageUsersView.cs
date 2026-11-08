using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly UserFileRepository userRepository;
    private ListUsersView listUsersView;
    private CreateUserView createUserView;
    private DeleteUserView deleteUserView;
    private UpdateUserView updateUserView;

    public ManageUsersView(UserFileRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void Options()
    {
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. View All Users");
        Console.WriteLine("3. Edit User");
        Console.WriteLine("4. Delete User");
        
        String? choice =  Console.ReadLine();

        switch (choice)
        {
            case null : 
                Console.WriteLine("That option doesnt exist");
                break;
            case "1" :
                createUserView = new CreateUserView(userRepository);
                createUserView.createUser();
                break;
            case "2" :
                listUsersView = new ListUsersView(userRepository);
                listUsersView.ShowAllUsers();
                break;
            case "3" :
                updateUserView = new UpdateUserView(userRepository);
                updateUserView.updateUser();
                break;
            case "4" :
                deleteUserView = new DeleteUserView(userRepository);
                deleteUserView.DeleteUser();
                break;
        }
    }
}