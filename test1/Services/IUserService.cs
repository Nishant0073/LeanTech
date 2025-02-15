namespace test1;

public interface IUserService
{
    public Task<string> GenerateToken(LoginModel loginModel);
    public Task<string> AddRole(RoleModel model);
    public Task<string> RegisterUser(LoginModel model);

}
