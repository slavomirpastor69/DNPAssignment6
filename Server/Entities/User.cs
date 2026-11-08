namespace Entities;

public class User
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public override string ToString()
    {
        return "ID: "+ Id 
                     + ", User Name: " + UserName
                     + ", Password: " + Password;
    }
}