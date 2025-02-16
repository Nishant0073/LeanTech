using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Test2;

public enum RolesEnum{
    Admin,
    User,
    Guest
}
public class Constants
{
    public static string DefaultUserName = "nishant@mail.com";
    public static string DefaultEmail = "nishant@mail.com";
    public static string DefaultPassword = "Pass@123";
    public static string DefaultRole = RolesEnum.User.ToString();

}
