namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
}
