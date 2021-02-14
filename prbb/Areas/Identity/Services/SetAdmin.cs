using Microsoft.AspNetCore.Identity;
using ModelsLayer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prbb.Areas.Identity.Services
{
    public static class SetAdmin
    {
        public static async Task AdminAsync(UserManager<ApplicationUser> userManager)
        {
            var User= await userManager.FindByEmailAsync("ADMIN@ADMIN.COM");
            if(User==null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    NormalizedUserName= "ADMIN@ADMIN.COM",
                    Email = "admin@admin.com",
                    NormalizedEmail="ADMIN@ADMIN.COM"
                };

                var result = await userManager.CreateAsync(user, "654321");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
