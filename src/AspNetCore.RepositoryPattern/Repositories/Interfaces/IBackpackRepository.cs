using AspNetCore.RepositoryPattern.Models;

namespace AspNetCore.RepositoryPattern.Repositories.Interfaces;

public interface IBackpackRepository : IGenericRepository<Backpack>
{

    Task<Backpack?> FindByUsernameAndPassword(string username, string password);
}
