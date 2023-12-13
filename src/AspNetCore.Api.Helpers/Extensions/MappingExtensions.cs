using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Api.Helpers.Extensions;

public static class MappingExtensions
{
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable, AutoMapper.IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}
