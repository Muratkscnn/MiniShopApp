using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MiniShopApp.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, ICardService cardService, IConfiguration configuration)
        {
            var roles = configuration.GetSection("UserData:Roles").GetChildren().Select(r => r.Value).ToArray();
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var users = configuration.GetSection("UserData:Users");
            foreach (var section in users.GetChildren())
            {
                var userName = section.GetValue<string>("UserName");
                var password = section.GetValue<string>("Password");
                var email = section.GetValue<string>("Email");
                var role = section.GetValue<string>("Role");
                var firstName = section.GetValue<string>("FirstName");
                var lastName = section.GetValue<string>("LastName");
                if (await userManager.FindByNameAsync(userName)==null)
                {
                    var user = new User()
                    {
                        UserName = userName,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EmailConfirmed=true
                    };
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                        cardService.InitializeCard(user.Id);
                    }
                }
            }
        }
    }
}
