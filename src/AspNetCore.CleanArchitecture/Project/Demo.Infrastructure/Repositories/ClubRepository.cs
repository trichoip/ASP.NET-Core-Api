using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Repositories;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly IGenericRepository<Club> _repository;

    public ClubRepository(IGenericRepository<Club> repository)
    {
        _repository = repository;
    }
}
