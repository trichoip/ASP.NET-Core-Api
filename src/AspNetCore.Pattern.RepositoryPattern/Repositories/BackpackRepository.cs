using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Models;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;
using System.Linq.Expressions;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

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
    public override Task<IEnumerable<Backpack>> FindAsync(Expression<Func<Backpack, bool>> expression, CancellationToken cancellationToken = default) => base.FindAsync(expression, cancellationToken);

    public override Task<Backpack?> FindByIdAsync(int id, CancellationToken cancellationToken = default) => base.FindByIdAsync(id, cancellationToken);

    public override Task<Backpack?> FindByIdAsync(object?[] index, CancellationToken cancellationToken = default) => base.FindByIdAsync(index, cancellationToken);

}
