namespace AspNetCore.Api.Record;

public record Member
{
    public int Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Address { get; init; } = default!;
}
