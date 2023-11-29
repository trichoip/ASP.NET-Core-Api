using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Common;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;

public sealed class User : BaseEntity
{

    public int Id { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

}
