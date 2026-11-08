using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class DeleteUserView
{
    private readonly UserFileRepository userRepository;

    public DeleteUserView(UserFileRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void DeleteUser()
    {
        Console.WriteLine("Which user should be deleted?: ");
        int id =  int.Parse(Console.ReadLine());
        userRepository.DeleteAsync(id);
    }
}