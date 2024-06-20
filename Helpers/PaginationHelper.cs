using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace shoolnew.Helpers
{
    public static class PaginationHelper
    {
        public static PaginatedList<T> Paginate<T>(
            IQueryable<T> source,
            int currentPage,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (filter != null)
            {
                source = source.Where(filter);
            }

            int totalItems = source.Count();

            if (orderBy != null)
            {
                source = orderBy(source);
            }

            List<T> items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, totalItems, currentPage, pageSize);
        }
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; private set; }
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public PaginatedList(List<T> items, int totalItems, int currentPage, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
