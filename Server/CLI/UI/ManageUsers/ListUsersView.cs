using RepositoryContracts;
using Entities;
using FileRepositories;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly UserFileRepository userRepository;
    
    public ListUsersView(UserFileRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void ShowAllUsers()
    {
        List<User> users = userRepository.GetMany().ToList() ;
        for (int i = 0; i < users.Count; i++)
        {
            Console.WriteLine(users[i]);
        }
    }
}