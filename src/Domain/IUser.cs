using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public interface IUser
{
    string Id { get; }
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    string UserName { get; }
    bool IsActive { get; }
    bool EmailConfirmed { get; }
}