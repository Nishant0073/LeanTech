namespace Test2;

public interface IUserService
{
    public Task<string> GetToken(LoginModel loginModel);
    public Task<string> AddRoles(AddRolesModel addRolesModel);
    public Task<string> RegisterUser(RegisterModel registerModel);

}
