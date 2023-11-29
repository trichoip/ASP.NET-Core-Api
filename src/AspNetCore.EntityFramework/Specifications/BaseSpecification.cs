using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AspNetCore.EntityFramework.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public BaseSpecification(Expression<Func<T, bool>>? Criteria)
        {
            this.Criteria = Criteria;
        }

        public List<Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>>> Includes { get; } = new();

        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; private set; }

        protected void AddInclude(Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public void AddOrderBy(Expression<Func<T, object>> OrderByexpression)
        {
            OrderBy = a => a.OrderBy(OrderByexpression);
        }
        public void AddOrderByDecending(Expression<Func<T, object>> OrderByDecending)
        {
            OrderBy = a => a.OrderByDescending(OrderByDecending);
        }

        public void AddFilter(Expression<Func<T, bool>> expression)
        {
            //Criteria = Criteria.AndAlso(expression);
        }
    }
}
