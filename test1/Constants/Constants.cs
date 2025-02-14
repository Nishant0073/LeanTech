using Org.BouncyCastle.Asn1.Crmf;

namespace test1;

public class Constants
{
    public enum Roles{
        User,
        Admin
    }
    public const Roles defaultRole = Roles.User;
    public const string userName = "nishant@gmail.com";
    public const string email = "nishant@gmail.com";
    public const string password = "Pass@123";

}
