using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}