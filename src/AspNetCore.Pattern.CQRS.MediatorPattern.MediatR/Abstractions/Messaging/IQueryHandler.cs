using MediatR;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}