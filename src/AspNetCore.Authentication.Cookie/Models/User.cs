namespace AspNetCore.Authentication.Cookie.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; }
    public string Role { get; set; } = string.Empty;
}
