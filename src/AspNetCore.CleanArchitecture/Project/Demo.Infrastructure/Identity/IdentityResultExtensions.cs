using AspNetCore.CleanArchitecture.Project.Demo.Shared;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result2 ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result2.Success()
            : Result2.Failure(result.Errors.Select(e => e.Description));
    }
}
