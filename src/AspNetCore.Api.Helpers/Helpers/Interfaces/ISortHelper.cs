namespace AspNetCore.Api.Helpers.Helpers.Interfaces;
public interface ISortHelper<T>
{
    IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
}
