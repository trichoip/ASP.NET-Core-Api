using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common;

public abstract record BaseEvent : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
