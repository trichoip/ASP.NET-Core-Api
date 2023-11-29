using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}