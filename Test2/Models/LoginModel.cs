using System.ComponentModel.DataAnnotations;

namespace Test2;

public class LoginModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }

}
