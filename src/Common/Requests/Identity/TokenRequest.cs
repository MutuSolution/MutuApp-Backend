using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Requests.Identity;

public class TokenRequest
{
    [DefaultValue("yunus")]
    public string UserName { get; set; }

    [DefaultValue("yunus@mail.com"), EmailAddress]
    public string Email { get; set; }

    [DefaultValue("12345678"), Required]
    public string Password { get; set; }
}
