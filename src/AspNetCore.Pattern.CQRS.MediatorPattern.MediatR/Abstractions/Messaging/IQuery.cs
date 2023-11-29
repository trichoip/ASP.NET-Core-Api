using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}