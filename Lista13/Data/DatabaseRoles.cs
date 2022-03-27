using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;

namespace Lista13.Data
{
    public class DatabaseRoles
    {
        public static string ADMIN = "Admin";
        public static string USER = "User";
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(ADMIN).Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = ADMIN
                };
                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.RoleExistsAsync(USER).Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = USER
                };
                roleManager.CreateAsync(role).Wait();
            }
        }

        public static void SeedAdmin(UserManager<IdentityUser> userManager)
        {
            string userName = Environment.GetEnvironmentVariable("ADMINLOGIN");
            if (userManager.FindByNameAsync(userName).Result == null)
            {
                string password = Environment.GetEnvironmentVariable("ADMINPASSWORD");
                IdentityUser user = new IdentityUser()
                {
                    Email = userName,
                    UserName = userName
                };
                IdentityResult result = userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ADMIN).Wait();
                } 
            }
        }
    }
}
