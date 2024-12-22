using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.Pagination;

public class LinksByUserNameParameters : PaginationParams
{
    public int MinLikeCount { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
    public string UserName { get; set; }
    public LinksByUserNameParameters()
    {
        OrderBy = "id";
    }
}