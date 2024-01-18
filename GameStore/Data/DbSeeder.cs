using Microsoft.AspNetCore.Identity;
using System;
using GameStore.Constants;

namespace GameStore.Data

{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            //role do bazy danych
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //admin

            var admin = new IdentityUser()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };
            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb == null)
            {
                await userMgr.CreateAsync(admin, "Admin123!");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
                await userMgr.AddToRoleAsync(admin, Roles.User.ToString());
            }
        }

    }
}
