using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Test2;

public class ApplicationSeedData
{
    public static async Task SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        roleManager.CreateAsync(new IdentityRole(RolesEnum.Admin.ToString())).Wait();
        roleManager.CreateAsync(new IdentityRole(RolesEnum.User.ToString())).Wait();
        roleManager.CreateAsync(new IdentityRole(RolesEnum.Guest.ToString())).Wait();

        var user = new IdentityUser{
            UserName = Constants.DefaultUserName,
            Email = Constants.DefaultEmail,
            EmailConfirmed = true
        };

        if(await userManager.FindByEmailAsync(Constants.DefaultEmail) == null)
        {
            var result = await userManager.CreateAsync(user, Constants.DefaultPassword);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Constants.DefaultRole);
            }
        }
    }
}
