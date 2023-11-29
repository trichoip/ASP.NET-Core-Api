using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}