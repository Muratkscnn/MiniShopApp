using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Identity
{
    public class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration) 
        {
            var adminUserName = configuration["UserData:AdminUser:UserName"];
            var adminPassword = configuration["UserData:AdminUser:Password"];
            var adminEmail = configuration["UserData:AdminUser:Email"];
            var adminRole = configuration["UserData:AdminUser:Role"];
            if (await userManager.FindByNameAsync(adminUserName)==null)
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
                var adminUser = new User()
                {
                    FirstName = "Admin",
                    LastName = "AdminOğlu",
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result=await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            var customerUserUserName = configuration["UserData:CustomerUser:UserName"];
            var customerUserPassword = configuration["UserData:CustomerUser:Password"];
            var customerUserEmail = configuration["UserData:CustomerUser:Email"];
            var customerUserRole = configuration["UserData:CustomerUser:Role"];
            if (await userManager.FindByNameAsync(customerUserUserName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(customerUserRole));
                var customerUser = new User()
                {
                    FirstName = "Customer",
                    LastName = "CustomerOğlu",
                    UserName = customerUserUserName,
                    Email = customerUserEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(customerUser, customerUserPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, customerUserRole);
                }
            }
        }
    }
}
