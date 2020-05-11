using Collateral.SupplierPortal.Apis.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework
{
    public static class IQueryableExtend
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query,
                                         int page = 0, int pageSize = 50) where T : class
        {
            if (page < 0)
                page = 0;
            else if (pageSize < 9)
                pageSize = 10;

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            if (skip <= 0)
                skip = 0;

            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync().ConfigureAwait(false);
            result.Results = result.Results.ToList();
            return result;
        }

        public static async Task<PagedResult<T>> GetPaged<T>(this IEnumerable<T> query,
                                         int page = 1, int pageSize = 50) where T : class
        {
            if (page < 1)
                page = 1;

            if (pageSize < 9)
                pageSize = 10;

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            if (skip <= 0)
                skip = 0;

            result.Results = await Task.Run(() =>
            {
                return query.Skip(skip).Take(pageSize);
            }).ConfigureAwait(false);

            return result;
        }
    }
}