namespace AspNetCore.CleanArchitecture.Project.Demo.Shared;

public class Result2
{
    internal Result2(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; init; }

    public string[] Errors { get; init; }

    public static Result2 Success()
    {
        return new Result2(true, Array.Empty<string>());
    }

    public static Result2 Failure(IEnumerable<string> errors)
    {
        return new Result2(false, errors);
    }
}
