using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AspNetCore.Database.EntityFramework.Specifications;

public interface ISpecifications<T>
{
    Expression<Func<T, bool>>? Criteria { get; set; }
    List<Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>> Includes { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
}
