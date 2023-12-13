using AspNetCore.Api.Helpers.Helpers;
using AutoMapper;
using System.Linq.Expressions;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{

    #region Get one entity to update or delete (chỉ dành cho việc lấy ra để update, delete chứ không lấy ra để hiển thị)
    // không nên dùng mấy hàm dưới để lấy ra rùi map sang dto,
    // nếu lấy ra mà map sang dto thì nếu trong dto có navigation property child thì nó sẽ viết thêm query để lấy ra child
    // nếu trong dto có navigation property list child1 mà trong child1 có navigation property child2 thì list child1 có 100 phần tử thì sẽ viết thêm 100 query để lấy ra child2
    // nếu muốn lấy ra entity để map sang dto thì nên dùng hàm FindByAsync<TDTO> trong region Get to mapper dto
    // hoặc hàm FindToIQueryableAsync sau đó _mapper.ProjectTo<TDTO>(queryable).FirstOrDefaultAsync();
    Task<T?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> FindByIdAsync(object?[] index, CancellationToken cancellationToken = default);
    Task<T?> FindOneAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null,
        CancellationToken cancellationToken = default);

    Task<T?> FindByAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default);
    #endregion

    #region Not performance (không nên sài)
    // vì khi lấy ra list entity thì cũng phải map sang dto,
    // khi map sang dto mà nếu trong dto có navigation property child thì nếu list entity có 100 phần tử thì sẽ viết thêm 100 query để lấy ra child
    Task<IEnumerable<T>> FindAllAsync(
        CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default);

    // hàm này lại càng không nên sài vì nó return PaginatedList entity
    // mà nếu map sang PaginatedList dto thì không thể dùng automapper để map được 
    // nếu map chỉ có thể dùng extension MapTo trong PaginatedListExtensions 
    // mà nếu dùng MapTo thì nếu trong dto có navigation property child thì nếu list entity có 100 phần tử thì sẽ viết thêm 100 query để lấy ra child
    Task<(int, PaginatedList<T>)> FindAsync(
        int pageIndex = 0,
        int pageSize = 0,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default);
    #endregion

    #region Bước đường cùng mới sài
    IQueryable<T> Entities { get; }
    Task<IQueryable<T>> FindToIQueryableAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
    #endregion

    #region Exist entity
    Task<bool> ExistsByAsync(
        Expression<Func<T, bool>>? expression = null,
        CancellationToken cancellationToken = default);
    #endregion

    #region Get to mapper dto
    Task<TDTO?> FindByAsync<TDTO>(
        AutoMapper.IConfigurationProvider configuration,
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task<IList<TDTO>> FindAsync<TDTO>(
        AutoMapper.IConfigurationProvider configuration,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task<PaginatedList<TDTO>> FindAsync<TDTO>(
        AutoMapper.IConfigurationProvider configuration,
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class;

    // trong IQueryable nếu Select thì include sẽ không có tác dụng
    // vì vậy includeFunc trong hàm FindWithPaginationAsync sẽ không có tác dụng vì bản chất ProjectTo là viết select dto (Select(new Dto{...})) nên includeFunc sẽ không có tác dụng
    // => includeFunc bị thừa thãi trong hàm FindWithPaginationAsync
    Task<PaginatedList<TDto>> FindWithPaginationAsync<TDto>(
        IMapper mapper,
        int pageIndex = 0,
        int pageSize = 0,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default) where TDto : class;
    #endregion

    #region Get to mapper dto - Mapster

    Task<TDTO?> FindByAsync<TDTO>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task<IList<TDTO>> FindAsync<TDTO>(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task<PaginatedList<TDTO>> FindAsync<TDTO>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class;

    #endregion

    #region Update, create, delete entity
    Task UpdateAsync(T entity);
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    #endregion
}
