﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Identity;

public class EmailConfirmRequest
{
    [Required,EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Code { get; set; }

}
