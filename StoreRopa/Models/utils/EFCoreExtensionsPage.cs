using Microsoft.EntityFrameworkCore;

namespace StoreRopa.Models.utils
{
    public static class EFCoreExtensionsPage
    {
        public static async Task<PagedResult<TEntity>> GetPagedResultAsync<TEntity>(
            this IQueryable<TEntity> source, int pageSize, int currentPage)
        where TEntity : class
        {
            var rows = source.Count();
            var results = await source
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TEntity>
            {
                CurrentPage = currentPage,
                PageCount = (int)Math.Ceiling((double)rows / pageSize),
                PageSize = pageSize,
                Results = results,
                RowsCount = rows
            };
        }

    }
}
