using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Authentication.Identity.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }

}
