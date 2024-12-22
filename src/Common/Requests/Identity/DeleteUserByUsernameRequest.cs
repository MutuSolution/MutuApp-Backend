using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Identity;

public class DeleteUserByUsernameRequest
{
    public string UserName { get; set; }
}
