using AspNetCore.CleanArchitecture.Project.Demo.Application.Common.Mappings;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Features.Players.Queries.GetAllPlayers;

public class GetAllPlayersDto : IMapFrom<Player>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int ShirtNo { get; init; }
    public int HeightInCm { get; init; }
    public string FacebookUrl { get; init; }
    public string TwitterUrl { get; init; }
    public string InstagramUrl { get; init; }
    public int DisplayOrder { get; init; }
}
