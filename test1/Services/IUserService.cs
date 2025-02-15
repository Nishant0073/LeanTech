namespace test1;

public interface IUserService
{
    public Task<string> GenerateToken(LoginModel model);

    public Task<string> AddRole();
    public Task<string> RegisterUser();

}
