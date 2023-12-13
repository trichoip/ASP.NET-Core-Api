using System.ComponentModel;

namespace AspNetCore.Authentication.JWT.ViewModel;

public class UserDto
{
    [DefaultValue("admin")]
    public required string Username { get; set; } = string.Empty;

    [DefaultValue("admin")]
    public required string Password { get; set; } = string.Empty;
}
