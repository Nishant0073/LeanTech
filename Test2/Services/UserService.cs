
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Test2;

public class UserService : IUserService
{
    private UserManager<IdentityUser> _userManager;
    private IConfiguration _configuration;
    public UserService(UserManager<IdentityUser> userManager,IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    public Task<string> AddRoles(AddRolesModel addRolesModel)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetToken(LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if(user == null)
            return "User not found!";

        if(await _userManager.CheckPasswordAsync(user,loginModel.Password)){
            JwtSecurityToken token = await GetJwtSecurityToken(user);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        return "Passowrd is invalid!";

    }

    private async Task<JwtSecurityToken> GetJwtSecurityToken(IdentityUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoleClaims = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        foreach(var role in userRoleClaims){
              roleClaims.Add(new Claim(ClaimTypes.Role,role));
        }

        var  claims = new []{
            new Claim(JwtRegisteredClaimNames.Sub,user.Id),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        }.Union(userClaims).Union(roleClaims);
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials  = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            signingCredentials : credentials,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Int32.Parse(_configuration["Jwt:DurationInMinutes"]))
        );
    }

    public Task<string> RegisterUser(RegisterModel registerModel)
    {
        throw new NotImplementedException();
    }
}
