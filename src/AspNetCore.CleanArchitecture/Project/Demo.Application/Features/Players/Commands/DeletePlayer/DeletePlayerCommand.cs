using AspNetCore.CleanArchitecture.Project.Demo.Application.Common.Mappings;
using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Repositories;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using AspNetCore.CleanArchitecture.Project.Demo.Shared;
using AutoMapper;
using MediatR;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Features.Players.Commands.DeletePlayer
{
    public record DeletePlayerCommand : IRequest<Result<int>>, IMapFrom<Player>
    {
        public int Id { get; set; }

        public DeletePlayerCommand()
        {

        }

        public DeletePlayerCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeletePlayerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(DeletePlayerCommand command, CancellationToken cancellationToken)
        {
            var player = await _unitOfWork.Repository<Player>().GetByIdAsync(command.Id);
            if (player != null)
            {
                await _unitOfWork.Repository<Player>().DeleteAsync(player);
                player.AddDomainEvent(new PlayerDeletedEvent(player));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(player.Id, "Product Deleted");
            }
            else
            {
                return await Result<int>.FailureAsync("Player Not Found.");
            }
        }
    }
}
