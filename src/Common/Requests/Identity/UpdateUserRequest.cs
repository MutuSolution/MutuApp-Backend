﻿namespace Common.Requests.Identity;

public class UpdateUserRequest
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
}
