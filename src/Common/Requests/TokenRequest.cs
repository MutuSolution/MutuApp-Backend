using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Common.Requests;

public class TokenRequest
{
    [DefaultValue("yunus@mail.com")]
    public string Email { get; set; }
    [DefaultValue("12345678")]
    public string Password { get; set; }
}
