namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services;

public interface IDateTimeService
{
    DateTime NowUtc { get; }
}
