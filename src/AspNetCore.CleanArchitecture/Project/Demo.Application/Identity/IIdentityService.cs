using AspNetCore.CleanArchitecture.Project.Demo.Shared;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Identity;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result2 Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result2> DeleteUserAsync(string userId);
}
