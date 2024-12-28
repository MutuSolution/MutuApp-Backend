using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.Pagination;

public class LikesByUserNameParameters : PaginationParams
{
    public string UserName { get; set; }
    public LikesByUserNameParameters()
    {
        OrderBy = "id";
    }
}
