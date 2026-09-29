namespace demo_hello.Models;

public class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string? Token { get; set; }
}