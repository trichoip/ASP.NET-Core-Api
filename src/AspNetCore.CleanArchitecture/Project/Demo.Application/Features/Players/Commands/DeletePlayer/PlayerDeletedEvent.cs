using AspNetCore.CleanArchitecture.Project.Demo.Domain.Common;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Features.Players.Commands.DeletePlayer;

public class PlayerDeletedEvent : BaseEvent
{
    public Player Player { get; }

    public PlayerDeletedEvent(Player player)
    {
        Player = player;
    }
}
