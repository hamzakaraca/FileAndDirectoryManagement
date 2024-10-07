using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DİrectoryAndFileManagement
{
    public static class Paginater
    {
        public static PaginatedResult<T> Paginate<T>(List<T> items, int pageNumber, int pageSize)
        {
            var count = items.Count;
            var itemsOnPage = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedResult<T>
            {
                Items = itemsOnPage,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }
}
