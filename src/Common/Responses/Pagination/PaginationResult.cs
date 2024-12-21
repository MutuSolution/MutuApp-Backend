using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.Pagination;
public class PaginationResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int ItemsPerPage { get; set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page * ItemsPerPage < TotalCount;

    public PaginationResult(IEnumerable<T> items, int totalCount, int page, int itemsPerPage)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        ItemsPerPage = itemsPerPage;
    }
}
