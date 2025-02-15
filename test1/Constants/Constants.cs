using Org.BouncyCastle.Asn1.Crmf;

namespace test1;

public class Constants
{
    public enum Roles{
        User,
        Admin
    }
    public const Roles defaultRole = Roles.User;
    public const string defualtUserName = "nishant@gmail.com";
    public const string defaultEmail = "nishant@gmail.com";
    public const string defaultPassword = "Pass@123";

}
