using MediatR;

namespace AspNetCore.CleanArchitecture.Project.Demo.Domain.Common;

public abstract class BaseEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
