using AspNetCore.CleanArchitecture.Project.Demo.Domain.Common;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Features.Players.Commands.UpdatePlayer
{
    public class PlayerUpdatedEvent : BaseEvent
    {
        public Player Player { get; }

        public PlayerUpdatedEvent(Player player)
        {
            Player = player;
        }
    }
}
