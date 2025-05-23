﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Admin;

public class AdminUpdateUserRequest
{
    public string UserId { get; set; } // Zorunlu
    public string? FirstName { get; set; } // Boş olabilir
    public string? LastName { get; set; } // Boş olabilir
    public bool IsActive { get; set; } // Zorunlu
    public bool IsEmailChange { get; set; } = true;
    public string? Email { get; set; } // Boş olabilir
    public bool? EmailConfirmed { get; set; } // Boş olabilir
    public bool IsUserNameChange { get; set; } = true;
    public string? UserName { get; set; } // Boş olabilir
    public bool IsPasswordChange { get; set; }
    public string? Password { get; set; } // Boş olabilir
}
