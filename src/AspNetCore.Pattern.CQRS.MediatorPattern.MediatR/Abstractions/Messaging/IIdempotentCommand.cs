namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;

public interface IIdempotentCommand<out TResponse> : ICommand<TResponse>
{
    Guid RequestId { get; set; }
}