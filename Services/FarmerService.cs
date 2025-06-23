using Microsoft.AspNetCore.Identity;
using PROG6212_New_POE.Models;
using System.Threading.Tasks;

namespace PROG6212_New_POE.Services
{
    public class FarmerService : IFarmerService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public FarmerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> AddFarmerAsync(ApplicationUser model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, "Default123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Farmer");
                return true;
            }
            return false;
        }
    }
} 