using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using PROG6212_New_POE.Data;
using PROG6212_New_POE.Models;

namespace PROG6212_New_POE.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            // Only create roles
            string[] roles = { "Admin", "Employee", "Farmer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
