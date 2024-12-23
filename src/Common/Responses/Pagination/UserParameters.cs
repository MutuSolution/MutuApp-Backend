using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.Pagination;

public class UserParameters : PaginationParams
{
    public bool? IsActive { get; set; }

    public UserParameters()
    {
        OrderBy = "UserName";
    }
}
