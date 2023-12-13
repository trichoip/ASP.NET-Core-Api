using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Repositories;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly IGenericRepository<Player> _repository;

    public PlayerRepository(IGenericRepository<Player> repository)
    {
        _repository = repository;
    }

    public async Task<List<Player>> GetPlayersByClubAsync(int clubId)
    {
        return await _repository.Entities.Where(x => x.ClubId == clubId).ToListAsync();
    }
}
