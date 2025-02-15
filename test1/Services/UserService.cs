using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;
using SshNet.Security.Cryptography;

namespace test1;

public class UserService: IUserService
{
    private UserManager<IdentityUser> _userManager;
    private ILogger<UserService> _logger;
    private IConfiguration _configuration;
    public UserService(UserManager<IdentityUser> userManager,ILogger<UserService> logger,IConfiguration configuration)
    {
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
    }

    public Task<string> AddRole()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateToken(LoginModel loginModel)
    {
        _logger.LogInformation("UserSercie.cs :: GenerateToken -> Started");
        try{
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if(user==null)
                return "User not found!";
            
            if(! await _userManager.CheckPasswordAsync(user,loginModel.Password)){
                return $"Invalid credentials for user {loginModel.Email}";
            }
            JwtSecurityToken jwtSecurityToken = await GetJwtToken(user);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }catch(Exception ex){
            return ex.Message;
        }

    }

    public Task<string> GenerateToken()
    {
        throw new NotImplementedException();
    }

    public Task<string> RegisterUser()
    {
        throw new NotImplementedException();
    }

    private async Task<JwtSecurityToken> GetJwtToken(IdentityUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userRole = new List<Claim>();
        
        foreach(var role in roles){
            userRole.Add(new Claim(ClaimTypes.Role,role));
        }
        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        }.Union(userClaims).Union(userRole);
        
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        var signingCredentials = new SigningCredentials(symmetricKey,SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(
            issuer: _configuration["Jwt:issuer"],
            audience: _configuration["Jwt:audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Int32.Parse(_configuration["Jwt:DurationInMinute"]?? "10")),
            signingCredentials: signingCredentials
        );
            
    }
}
