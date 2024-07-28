using Microsoft.AspNetCore.Identity;
using OnlineLearningPlatform.Context.Identity;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = { "Admin", "Student", "Guest" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Add Admin user
        var adminUser = new ApplicationUser
        {
            UserName = "admin@plat.com",
            Email = "admin@plat.com",
            FirstName = "Admin",
            LastName = "User"
        };

        string adminPassword = "Admin@123";
        var admin = await userManager.FindByEmailAsync(adminUser.Email);

        if (admin == null)
        {
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
