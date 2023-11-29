using AspNetCore.Pattern.RepositoryPattern.Models;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;

public interface IBackpackRepository : IGenericRepository<Backpack>
{

    Task<Backpack?> FindByUsernameAndPassword(string username, string password);
}
