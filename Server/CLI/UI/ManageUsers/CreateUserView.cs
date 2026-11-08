using RepositoryContracts;
using Entities;
using FileRepositories;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly UserFileRepository userRepository;

    public CreateUserView(UserFileRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void createUser()
    {
        Console.Write("Username: ");
        String username = Console.ReadLine();
        Console.Write("Password: ");
        String password = Console.ReadLine();
        User user = new User(username, password);
        userRepository.AddAsync(user);
    }
}