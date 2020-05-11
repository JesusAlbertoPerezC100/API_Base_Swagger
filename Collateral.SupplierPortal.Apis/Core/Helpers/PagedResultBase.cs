using System;
using System.Collections.Generic;

namespace Collateral.SupplierPortal.Apis.Core.Helpers
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get
            {
                var count = (CurrentPage - 1) * PageSize + 1;

                if (count < 0)
                    count = 0;
                return count;
            }
        }

        public int LastRowOnPage
        {
            get
            {
                var count = Math.Min(CurrentPage * PageSize, RowCount);

                if (count < 0)
                    count = 0;

                return count;
            }
        }
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}