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

public class UserService : IUserService
{
    private UserManager<IdentityUser> _userManager;
    private ILogger<UserService> _logger;
    private IConfiguration _configuration;
    public UserService(UserManager<IdentityUser> userManager, ILogger<UserService> logger, IConfiguration configuration)
    {
        _userManager = userManager;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<string> AddRole(RoleModel model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return "No user registered with this email";
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roleExists = Enum.GetNames(typeof(Constants.Roles))
                    .Any(x => x.ToLower() == model.role.ToLower());
                if (roleExists)
                {
                    var validRole = Enum.GetValues(typeof(Constants.Roles)).Cast<Constants.Roles>().Where(x => x.ToString().ToLower() == model.role.ToLower()).FirstOrDefault();
                    await _userManager.AddToRoleAsync(user, validRole.ToString());
                    return $"Added {model.role} to user {model.Email}";
                }
            }
                return "Failed to add role!";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<string> GenerateToken(LoginModel loginModel)
    {
        _logger.LogInformation("UserSercie.cs :: GenerateToken -> Started");
        try
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                return "User not found!";

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return $"Invalid credentials for user {loginModel.Email}";
            }
            JwtSecurityToken jwtSecurityToken = await GetJwtToken(user);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }

    public async Task<string> RegisterUser(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
            return "User exist already";

        var newUser = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
        var isCreated = await _userManager.CreateAsync(newUser, model.Password);
        if (isCreated.Succeeded)
        {
            return "User created succesfully!";
        }
        return $"Error while creating user: {isCreated.Errors.FirstOrDefault()}";
    }

    private async Task<JwtSecurityToken> GetJwtToken(IdentityUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var userRole = new List<Claim>();

        foreach (var role in roles)
        {
            userRole.Add(new Claim(ClaimTypes.Role, role));
        }
        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        }.Union(userClaims).Union(userRole);

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        return new JwtSecurityToken(
            issuer: _configuration["Jwt:issuer"],
            audience: _configuration["Jwt:audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Int32.Parse(_configuration["Jwt:DurationInMinute"] ?? "10")),
            signingCredentials: signingCredentials
        );

    }
}
