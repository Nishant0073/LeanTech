using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace test1;

public class ApplicationSeed
{
    public static async Task SeedData(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager){
        await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Constants.Roles.User.ToString()));
        var defaultUser =  new IdentityUser{UserName = Constants.defualtUserName, Email = Constants.defaultEmail,EmailConfirmed = true};

        if(userManager.Users.All(e => e.Email!=Constants.defaultEmail)){

            var result = await userManager.CreateAsync(defaultUser,Constants.defaultPassword);
            if(result.Succeeded){
                await userManager.AddToRoleAsync(defaultUser,Constants.defaultRole.ToString());
            }{
                throw new Exception($"Failed to seed data {result.Errors.FirstOrDefault().Description}");
            }
        }
    }

}
