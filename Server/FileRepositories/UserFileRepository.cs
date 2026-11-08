using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        int maxId = users.Count > 0 ? users.Max(p => p.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        users ??= [];
        
        User? existingUser =  users.FirstOrDefault(p => p.Id == user.Id);

        if (existingUser == null)
        {
            throw new NotFoundException($"User with ID {user.Id} was not found.");
        }

        users.Remove(existingUser!);
        users.Add(user);
        
        usersAsJson = JsonSerializer.Serialize(users);
        
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        users ??= [];
        
        User? existingUser =  users.FirstOrDefault(p => p.Id == id);

        if (existingUser == null)
        {
            throw new NotFoundException($"User with ID {id} was not found.");
        }

        users.Remove(existingUser!);
        
        usersAsJson = JsonSerializer.Serialize(users);
        
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }
    
    public async Task<User> GetSingleAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        users ??= [];
        
        User? existingUser =  users.FirstOrDefault(p => p.Id == id);

        if (existingUser == null)
        {
            throw new NotFoundException($"User with ID {id} was not found.");
        }

        return existingUser!;
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User>? users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        users ??= [];
        
        return users.AsQueryable();
    }
}