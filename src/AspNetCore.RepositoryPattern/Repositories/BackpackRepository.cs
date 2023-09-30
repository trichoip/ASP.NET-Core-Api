using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Models;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using System.Linq.Expressions;

namespace AspNetCore.RepositoryPattern.Repositories;

public class BackpackRepository : GenericRepository<Backpack>, IBackpackRepository
{
    private readonly DataContext context;
    public BackpackRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }

    public async Task<Backpack?> FindByUsernameAndPassword(string username, string password)
    {
        return await FindOneAsync(c => c.Description == username);
    }

    // nếu muốn overide thì sửa lại cái này còn nếu không thì xóa đi hoặc để là base.function(entity);
    public override Task<IEnumerable<Backpack>> FindAsync(Expression<Func<Backpack, bool>> expression) => base.FindAsync(expression);

    public override Task<Backpack?> FindByIdAsync(int id) => base.FindByIdAsync(id);

    public override Task<Backpack?> FindByIdAsync(object?[] index) => base.FindByIdAsync(index);

}
