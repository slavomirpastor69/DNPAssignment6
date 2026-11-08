namespace ApiContracts;

public class CreateUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class UpdateUserDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
}
